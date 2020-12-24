using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WT.FileScanner.Shared.Models;

namespace WT.FileScanner.UI.Shared.ViewModels
{
    public class FileScannerViewModel : BaseUpdateViewModel
    {
        private Stopwatch scannerStopwatch = new Stopwatch();
        private CancellationTokenSource scannerToken = new CancellationTokenSource();

        public FileScannerViewModel()
        {
            StartScanCommand = new AsyncRelayCommand(StartScan);
            OpenExplorerCommand = new RelayCommand<string>(OpenExplorer);
        }

        public ScanProfile CurrentScanProfile { get; set; }
        public ObservableCollection<DirectoryInfo> ScannedDirectories { get; set; }
        public ObservableCollection<FileInfo> ScannedFiles { get; set; }
        public ICommand StartScanCommand { get; set; }
        public ICommand OpenExplorerCommand { get; set; }

        public void OpenExplorer(string fullPath)
        {
            try
            {
                Process.Start("explorer.exe", Path.GetDirectoryName(fullPath));
            }
            catch (Exception ex)
            {
                ShowAlert($"Error: {ex.Message}");
            }
        }

        public override Task OnUpdate(CancellationToken token)
        {
            //This is where we will gather the users scanning profile from the local database made by EF Core
            throw new NotImplementedException();
        }

        protected void ClearPreviousScan()
        {
            ScannedDirectories.Clear();
            ScannedFiles.Clear();
        }

        protected Task StartScan()
        {
            ClearPreviousScan();
            scannerStopwatch.Restart();
            return Task.CompletedTask;
        }
    }
}