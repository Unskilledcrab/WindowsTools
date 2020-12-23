using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WT.FileScanner.Shared.Models;
using WT.FileScanner.UI.Shared.ViewModels;

namespace WT.FileScanner.WPF.ViewModels
{
    public class FileScannerViewModel : BaseViewModel
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
                MessageBox.Show($"Error: {ex.Message}", "Error");
            }
        }

        public override Task OnUpdate(CancellationToken token)
        {
            //This is where we will gather the users scanning profile from their local settings
            throw new NotImplementedException();
        }

        protected void ClearScan()
        {
            ScannedDirectories.Clear();
            ScannedFiles.Clear();
        }

        protected Task StartScan()
        {
            ClearScan();
            scannerStopwatch.Restart();
            return Task.CompletedTask();
        }
    }
}