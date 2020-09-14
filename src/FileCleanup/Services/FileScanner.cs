using FileCleanup.Models;
using FileCleanup.ProgressModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FileCleanup.Extensions;
using System.Windows.Media;
using Newtonsoft.Json;
using System.Net.Http;

namespace FileCleanup.Services
{
    public class FileScanner
    {
        #region Properties
        public ObservableCollection<FileProps> FlaggedFiles { get; private set; } = new ObservableCollection<FileProps>();
        public ObservableCollection<FileProps> FlaggedDirectories { get; private set; } = new ObservableCollection<FileProps>();

        private CancellationTokenSource token = new CancellationTokenSource();
        private Stopwatch stopwatch = new Stopwatch();
        private Configuration Configuration;
        #endregion

        public event EventHandler ScanComplete;
        public event EventHandler<FileProps> DirectoryAdded;
        public event EventHandler<FileProps> FileAdded;
        public event EventHandler<TimeSpan> ScannerCancelled;

        #region Constructors
        public FileScanner(long flagFileSize, DateTime flagLastAccessDate)
        {
            Configuration = new Configuration
            {
                FlagFileSize = flagFileSize,
                LastAccessFlagDate = flagLastAccessDate
            };
        }

        public FileScanner(Configuration configuration)
        {
            Configuration = configuration;
        }
        #endregion

        public void UpdateConfiguration(Configuration configuration)
        {
            Configuration = configuration;
        }

        public void CancelScan()
        {
            token.Cancel();
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
                    OnScannerCancelled(timeElapsed);
                }
                catch (Exception ex)
                {

                }
            }
            OnScanComplete(EventArgs.Empty);
        }

        private async Task Scan(string path, IProgress<ScanProgress> progress, CancellationToken token)
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

        private bool CanAddFile(FileInfo fileInfo)
        {
            var lengthCheck = Configuration.IsOverFlagSize(fileInfo.Length);
            var accessTimeCheck = Configuration.IsOverFlagAccessDate(fileInfo.LastAccessTime);

            return (lengthCheck && accessTimeCheck);
        }

        private bool DoNotScan(string path)
        {
            return Configuration.PathsNotToScan.Contains(path);
        }

        #region Events
        protected virtual void OnScanComplete(EventArgs e)
        {
            ScanComplete?.Invoke(this, e);
        }

        protected virtual void OnFileAdded(FileProps file)
        {
            FileAdded?.Invoke(this, file);
        }

        protected virtual void OnDirectoryAdded(FileProps directory)
        {
            DirectoryAdded?.Invoke(this, directory);
        }

        protected virtual void OnScannerCancelled(TimeSpan cancelTime)
        {
            ScannerCancelled?.Invoke(this, cancelTime);
        }
        #endregion
    }
}
