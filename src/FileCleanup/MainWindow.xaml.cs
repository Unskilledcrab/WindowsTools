using FileCleanup.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FileCleanup
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ObservableCollection<FileProps> FlaggedFiles { get; set; } = new ObservableCollection<FileProps>();
        public ObservableCollection<FileProps> FlaggedDirectories { get; set; } = new ObservableCollection<FileProps>();

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

        public void UpdateConfiguration(string size)
        {
            Configuration.FlagFileSize = ConvertSizeToByte(Int32.Parse(size), ConvertStringToSizeType(SelectedSizeType));
        }

        public enum FileSizeType
        {
            Kb,
            Mb,
            Gb
        }

        private FileSizeType ConvertStringToSizeType(string sizeType)
        {
            Enum.TryParse(sizeType, out FileSizeType result);
            return result;
        }

        private long ConvertSizeToByte(long size, FileSizeType type)
        {
            switch (type)
            {
                case FileSizeType.Kb:
                    size *= 1024;
                    break;
                case FileSizeType.Mb:
                    size *= (long)Math.Pow(1024, 2);
                    break;
                case FileSizeType.Gb:
                    size *= (long)Math.Pow(1024, 3);
                    break;
                default:
                    size *= (long)Math.Pow(1024, 4);
                    break;
            }
            return size;
        }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            TestConfiguration();
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
                    await Task.Run(async () => { await Scan(driveInfo.Name, progress, token.Token); }, token.Token);
                }
                catch (OperationCanceledException)
                {
                    var timeElapsed = stopwatch.Elapsed;
                    ScanningStatus = $"Cancelled {timeElapsed:c}";
                }
                catch (Exception ex)
                {

                }
                finally
                {

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
                    await Task.Run(async () => { await ScanDirectory(directory, progress, token); }, token);
                }
                catch { }
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
            await Task.Run(async () => { await Scan(directory, progress, token); }, token);
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

        private CancellationTokenSource token = new CancellationTokenSource();

        private Stopwatch stopwatch = new Stopwatch();
        private async void StartScanButton_Click(object sender, RoutedEventArgs e)
        {
            token = new CancellationTokenSource();
            ScanningStatus = "Running...";
            stopwatch.Start();
            var progress = new Progress<ScanProgress>();
            progress.ProgressChanged += UpdateProgress;
            await StartScanner(progress);
            stopwatch.Stop();
            var timeElapsed = stopwatch.Elapsed;
            ScanningStatus = $"Complete in {timeElapsed:c}";
        }

        private void OpenExplorer(object sender, RoutedEventArgs e)
        {
            FileProps row = (FileProps)((Button)e.Source).DataContext;

            try
            {
                Process.Start("explorer.exe", System.IO.Path.GetDirectoryName(row.FullPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        private void AddToNoScanList(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.Source;
            FileProps row = (FileProps)button.DataContext;

            try
            {
                if (!Configuration.PathsNotToScan.Contains(row.FullPath))
                    Configuration.PathsNotToScan.Add(row.FullPath);

                row.IsScanable = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }

        }
        private void AddToScanList(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.Source;
            string row = (string)button.DataContext;

            try
            {
                if (Configuration.PathsNotToScan.Contains(row))
                {
                    Configuration.PathsNotToScan.Remove(row);
                    var dir = FlaggedDirectories.Where(f => f.FullPath == row).FirstOrDefault();
                    if (dir != null)
                        dir.IsScanable = true;
                    else
                    {
                        var file = FlaggedFiles.Where(f => f.FullPath == row).FirstOrDefault();
                        if (file != null)
                            file.IsScanable = true;
                    }                   

                    button.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error");
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

        private void CancelScanButton_Click(object sender, RoutedEventArgs e)
        {
            token.Cancel();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
