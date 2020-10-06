using FileCleanup.Helpers;
using GalaSoft.MvvmLight;
using System;
using System.IO;

namespace FileCleanup.Models
{
    public class FileProps : ViewModelBase
    {
        public string Name => Path.GetFileName(FullPath);
        public bool IsScanable
        {
            get { return isScanable; }
            set
            {
                isScanable = value;
                RaisePropertyChanged(nameof(IsScanable));
                RaisePropertyChanged(nameof(IsScanableStatus));
            }
        }
        public string IsScanableStatus => IsScanable ? "Don't Scan" : "Not Scanning";
        public string FullPath { get; set; }
        public FileType Type { get; set; }
        public DateTime LastAccessed { get; set; }
        public long ByteSize { get; set; }
        public long KiloByteSize => ByteSize / 1024;
        public string Size
        {
            get
            {
                var factor = 1024;
                var convertedSize = ByteSize;
                if (convertedSize < factor)
                    return $"{convertedSize} Bytes";
                else
                    convertedSize /= factor;

                if (convertedSize < factor)
                    return $"{convertedSize} Kb";
                else
                    convertedSize /= factor;

                if (convertedSize < factor)
                    return $"{convertedSize} Mb";
                else
                    convertedSize /= factor;

                return $"{convertedSize} Gb";
            }
        }
        public string DaysSinceViewed
        {
            get
            {
                var time = DateTime.Now.Subtract(LastAccessed);
                return $"{time.Days} days";
            }
        }

        private bool isScanable = true;

        public FileProps(FileInfo file)
        {
            FullPath = file.FullName;
            LastAccessed = file.LastAccessTime;
            ByteSize = file.Length;
            Type = Utils.GetFileTypeFromExtension(file.Extension);
        }

        public FileProps(DirectoryInfo directory)
        {
            FullPath = directory.FullName;
            LastAccessed = directory.LastAccessTime;
            ByteSize = 0;
            Type = FileType.directory;
        }
    }
}
