using FileCleanup.ProgressModels;
using FileCleanup.Helpers;
using FileCleanup.Models;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using FileCleanup.Services;
using FileCleanupDataLib.Models;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight;

namespace FileCleanup.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        #region View Properties

        public bool IsScanning { get; set;}
        public string ScanningStatus { get; set; } = "Not Started";

        private DateTime selectedDate = DateTime.Now.AddDays(-60);
        public DateTime SelectedDate
        {
            get => selectedDate;
            set
            {
                selectedDate = value;
                Configuration.LastAccessFlagDate = value;
            }
        }

        private string size = "1";
        public string Size
        {
            get => size;
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
            get => selectedSizeType;
            set
            {
                selectedSizeType = value;
                UpdateConfiguration(Size);
            }
        }

        public Configuration Configuration { get; set; }
        #endregion

        #region Model Properties
        public FileScanner FileScanner { get; }
        public ObservableCollection<CfgScanProfile> ScanProfiles { get; set; }
        private readonly IDialogService _dialogService;
        #endregion

        #region Commands
        public RelayCommand CancelCommand { get; private set; }
        public IAsyncCommand StartCommand { get; private set; }
        public IAsyncCommand<string> OpenExplorerCommand { get; set; }
        public IAsyncCommand<string> AddToScanListCommand { get; set; }
        public IAsyncCommand<FileProps> AddToNoScanListCommand { get; set; }
        public RelayCommand NewScanningProfileCommand { get; }
        #endregion

        public MainWindowViewModel()
        {
            CancelCommand = new RelayCommand(ExecuteCancelScanner, () => FileScanner.IsRunning);
            StartCommand = new AsyncCommand(ExecuteStartScanner, _ => !FileScanner.IsRunning);
            OpenExplorerCommand = new AsyncCommand<string>(OpenExplorer);
            AddToScanListCommand = new AsyncCommand<string>(AddToScanList);
            AddToNoScanListCommand = new AsyncCommand<FileProps>(AddToNoScanList);
            NewScanningProfileCommand = new RelayCommand(NewScanningProfile);
            TestConfiguration();
            FileScanner = new FileScanner(Configuration);
            ScanProfiles = new ObservableCollection<CfgScanProfile>();
            _dialogService = new DialogService();
        }

        public void UpdateConfiguration(string size)
        {
            Configuration.FlagFileSize = Utils.ConvertSizeToByte(int.Parse(size), Utils.ConvertStringToSizeType(SelectedSizeType));
            FileScanner.UpdateConfiguration(Configuration);
        }

        private void TestConfiguration()
        {
            Configuration = new Configuration
            {
                ScanSystemFolders = false,
                ScanProgramDataFolders = false,
                ScanProgramFolders = false,
                FlagFileSize = 100, //* 1000 * 1000, // Megabytes: regularly in bytes
                LastAccessFlagDate = SelectedDate
            };
        }

        public async Task ExecuteStartScanner()
        {
            var progress = new Progress<ScanProgress>();
            progress.ProgressChanged += UpdateProgress;
            try
            {
                await FileScanner.StartScanner(progress);
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
                    var dir = FileScanner.FlaggedDirectories.FirstOrDefault(f => f.FullPath == fullPath);
                    if (dir != null)
                        dir.IsScanable = true;
                    else
                    {
                        var file = FileScanner.FlaggedFiles.FirstOrDefault(f => f.FullPath == fullPath);
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
            if (FileScanner.IsRunning)
            {
                var timeElapsed = FileScanner.TimeElapsed;
                ScanningStatus = $"Running... {timeElapsed:c}";
            }
        }

        public void ExecuteCancelScanner()
        {
            FileScanner.CancelScan();
        }

        private void NewScanningProfile()
        {
            var viewModel = new ScanProfileDialogViewModel("Add new Scanning Profile", string.Empty);
            var newScanProfile = _dialogService.OpenDialog(viewModel);
            if (newScanProfile != null)
                ScanProfiles.Add(newScanProfile);
        }
    }
}
