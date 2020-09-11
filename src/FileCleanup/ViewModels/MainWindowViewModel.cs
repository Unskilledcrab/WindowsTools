using FileCleanup.ProgressModels;
using FileCleanup.Extensions;
using FileCleanup.Helpers;
using FileCleanup.Models;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;

namespace FileCleanup.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region View Properties
        public ObservableCollection<FileProps> FlaggedFiles { get; set; } = new ObservableCollection<FileProps>();
        public ObservableCollection<FileProps> FlaggedDirectories { get; set; } = new ObservableCollection<FileProps>();

        private bool isScanning;
        public bool IsScanning { get => isScanning; 
            private set 
            {
                isScanning = value;
                NotifyPropertyChanged(nameof(IsScanning));
            } 
        }

        private string scanningStatus = "Not Started";
        public string ScanningStatus
        {
            get { return scanningStatus; }
            set { scanningStatus = value; NotifyPropertyChanged(nameof(ScanningStatus)); }
        }

        private DateTime selectedDate = DateTime.Now.AddDays(-60);
        public DateTime SelectedDate
        {
            get { return selectedDate; }
            set
            {
                selectedDate = value;
                Configuration.LastAccessFlagDate = value;
            }
        }

        private string size = "1";
        public string Size
        {
            get { return size; }
            set
            {
                try
                {
                    UpdateConfiguration(value);
                    size = value;
                }
                catch (Exception)
                {
                    size = "1000";
                }
            }
        }

        private string selectedSizeType = "Gb";
        public string SelectedSizeType
        {
            get { return selectedSizeType; }
            set
            {
                selectedSizeType = value;
                UpdateConfiguration(Size);
            }
        }

        private Configuration configuration;
        public Configuration Configuration
        {
            get { return configuration; }
            set { configuration = value; NotifyPropertyChanged(nameof(Configuration)); }
        }
        #endregion

        #region Model Properties
        private CancellationTokenSource token = new CancellationTokenSource();
        private Stopwatch stopwatch = new Stopwatch();
        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; private set; }
        public IAsyncCommand StartCommand { get; private set; }
        public IAsyncCommand<string> OpenExplorerCommand { get; set; }
        public IAsyncCommand<string> AddToScanListCommand { get; set; }
        public IAsyncCommand<FileProps> AddToNoScanListCommand { get; set; }
        #endregion

        public MainWindowViewModel()
        {
            CancelCommand = new RelayCommand(ExecuteCancelScanner, () => IsScanning);
            StartCommand = new AsyncCommand(ExecuteStartScanner, _ => !IsScanning);
            OpenExplorerCommand = new AsyncCommand<string>(OpenExplorer);
            AddToScanListCommand = new AsyncCommand<string>(AddToScanList);
            AddToNoScanListCommand = new AsyncCommand<FileProps>(AddToNoScanList);
            //TestConfiguration();
        }

        public void UpdateConfiguration(string size)
        {
            Configuration.FlagFileSize = Utils.ConvertSizeToByte(Int32.Parse(size), Utils.ConvertStringToSizeType(SelectedSizeType));
        }

        private void TestConfiguration()
        {
            Configuration = new Configuration()
            {
                ScanSystemFolders = false,
                ScanProgramDataFolders = false,
                ScanProgramFolders = false,
                FlagFileSize = 100 * 1000 * 1000, // Megabytes: regularly in bytes
                LastAccessFlagDate = SelectedDate
            };
            UpdateConfiguration(Size);
        }

        public async Task StartScanner(IProgress<ScanProgress> progress)
        {
            FlaggedDirectories.Clear();
            FlaggedFiles.Clear();

            var drives = DriveInfo.GetDrives();
            foreach (var driveInfo in drives)
            {
                try
                {
                   await Scan(driveInfo.Name, progress, token.Token);
                }
                catch (OperationCanceledException)
                {
                    var timeElapsed = stopwatch.Elapsed;
                    ScanningStatus = $"Cancelled {timeElapsed:c}";
                }
            }
        }

        public async Task Scan(string path, IProgress<ScanProgress> progress, CancellationToken token)
        {
            if (token.IsCancellationRequested)
                token.ThrowIfCancellationRequested();

            foreach (var directory in Directory.GetDirectories(path))
            {
                try
                {
                    await ScanDirectory(directory, progress, token);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private async Task ScanDirectory(string directory, IProgress<ScanProgress> progress, CancellationToken token)
        {
            progress.Report(new ScanProgress(directory));

            if (DoNotScan(directory))
                return;

            var directoryToAdd = new DirectoryInfo(directory);
            if (CanAddDirectory(directoryToAdd))
                progress.Report(new ScanProgress(new FileProps(directoryToAdd), false));

            foreach (var file in Directory.GetFiles(directory))
            {
                if (token.IsCancellationRequested)
                    token.ThrowIfCancellationRequested();
                ScanFile(file, progress);
            }
            await Scan(directory, progress, token);
        }

        private void ScanFile(string file, IProgress<ScanProgress> progress)
        {
            if (DoNotScan(file))
                return;

            var fileToAdd = new FileInfo(file);
            if (CanAddFile(fileToAdd))
                progress.Report(new ScanProgress(new FileProps(fileToAdd), true));
        }

        private bool CanAddDirectory(DirectoryInfo directory)
        {
            var accessTimeCheck = Configuration.IsOverFlagAccessDate(directory.LastAccessTime);

            return (accessTimeCheck);
        }

        public bool CanAddFile(FileInfo fileInfo)
        {
            var lengthCheck = Configuration.IsOverFlagSize(fileInfo.Length);
            var accessTimeCheck = Configuration.IsOverFlagAccessDate(fileInfo.LastAccessTime);

            return (lengthCheck && accessTimeCheck);
        }

        public bool DoNotScan(string path)
        {
            return Configuration.PathsNotToScan.Contains(path);
        }
        
        public async Task ExecuteStartScanner()
        {
            IsScanning = true;
            token = new CancellationTokenSource();
            ScanningStatus = "Running...";
            stopwatch.Start();
            var progress = new Progress<ScanProgress>();
            progress.ProgressChanged += UpdateProgress;

            try
            {
                await Task.Run(() => StartScanner(progress));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }

            stopwatch.Stop();
            var timeElapsed = stopwatch.Elapsed;
            ScanningStatus = $"Complete in {timeElapsed:c}";
            IsScanning = false;
        }

        public async Task OpenExplorer(string fullPath)
        {
            try
            {
                Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(fullPath));
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private async Task AddToNoScanList(FileProps file)
        {
            try
            {
                if (!Configuration.PathsNotToScan.Contains(file.FullPath))
                    Configuration.PathsNotToScan.Add(file.FullPath);

                file.IsScanable = false;
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error: {ex.Message}", "Error");
            }

        }
        private async Task AddToScanList(string fullPath)
        {
            try
            {
                if (Configuration.PathsNotToScan.Contains(fullPath))
                {
                    Configuration.PathsNotToScan.Remove(fullPath);
                    var dir = FlaggedDirectories.Where(f => f.FullPath == fullPath).FirstOrDefault();
                    if (dir != null)
                        dir.IsScanable = true;
                    else
                    {
                        var file = FlaggedFiles.Where(f => f.FullPath == fullPath).FirstOrDefault();
                        if (file != null)
                            file.IsScanable = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void UpdateProgress(object sender, ScanProgress e)
        {
            if (e.IsUpdateStatus)
            {
                ScanningStatus = $"Scanning... {e.CurrentDirectory}";
                return;
            }
            if (stopwatch.IsRunning)
            {
                var timeElapsed = stopwatch.Elapsed;
                ScanningStatus = $"Running... {timeElapsed:c}";
            }

            if (e.IsFile)
                FlaggedFiles.Add(e.File);
            else
                FlaggedDirectories.Add(e.File);
        }

        public void ExecuteCancelScanner()
        {
            token.Cancel();
        }
    }
}
