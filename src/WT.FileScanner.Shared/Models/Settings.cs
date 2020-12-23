using System;
using WT.FileScanner.Shared.Extensions;

namespace WT.FileScanner.Shared.Models
{
    public class Settings : BaseEntity
    {
        public ScanProfile ScanProfile { get; set; }
        public long FlagFileSizeMin { get; set; } = 1.ConvertSizeToBytes(FileSizeType.Mb);
        public long FlagFileSizeMax { get; set; } = 50.ConvertSizeToBytes(FileSizeType.Gb);
        public DateTime FlagAccessDateTimeMin { get; set; } = DateTime.Now.AddDays(-60);
        public DateTime FlagAccessDateTimeMax { get; set; } = DateTime.Now;
        public bool ScanAllDrives { get; set; } = true;
        public bool ScanProgramFolders { get; set; }
        public bool ScanSystemFolders { get; set; }
        public bool ScanDownloadsFolder { get; set; }
        public bool ScanDocumentFolder { get; set; }
        public bool ScanPicturesFolder { get; set; }
        public bool ScanVideosFolder { get; set; }
        public bool ScanDesktopFolder { get; set; }
        public bool ScanMusicFolder { get; set; }
        public bool ScanAppDataFolder { get; set; }
    }
}