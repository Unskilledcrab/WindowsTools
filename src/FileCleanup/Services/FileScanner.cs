using FileCleanup.ProgressModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileCleanup.Services
{
    class FileScanner
    {
        public event EventHandler ScanComplete;
        public event EventHandler DirectoryAdded;
        public event EventHandler FileAdded;

        private CancellationTokenSource token = new CancellationTokenSource();
        private Stopwatch stopwatch = new Stopwatch();

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

            OnScanComplete(EventArgs.Empty);
        }

        protected virtual void OnScanComplete(EventArgs e)
        {
            ScanComplete?.Invoke(this, e);
        }
    }
}
