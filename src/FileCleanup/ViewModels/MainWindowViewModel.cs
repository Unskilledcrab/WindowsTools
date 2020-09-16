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
using FileCleanup.Services;

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
        private FileScanner fileScanner;
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
            TestConfiguration();
            fileScanner = new FileScanner(Configuration);
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
        
        public async Task ExecuteStartScanner()
        {
            IsScanning = true;
            var progress = new Progress<ScanProgress>();
            progress.ProgressChanged += UpdateProgress;
            try
            {
                await Task.Run(() =>fileScanner.StartScanner(progress));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
            }
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
            ////var scanner = (FileScanner)sender;
            ////if (scanner.IsRunning)
            ////{
            ////    var timeElapsed = scanner.TimeElapsed;
            ////    ScanningStatus = $"Running... {timeElapsed:c}";
            ////}

            ////if (e.IsFile)
            ////    FlaggedFiles.Add(e.File);
            ////else
            ////    FlaggedDirectories.Add(e.File);
        }

        public void ExecuteCancelScanner()
        {
            fileScanner.CancelScan();
        }
    }
}
