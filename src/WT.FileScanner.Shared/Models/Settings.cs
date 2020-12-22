using System;

namespace WT.FileScanner.Shared.Models
{
    public class Settings : BaseEntity
    {
        public ScanProfile ScanProfile { get; set; }
        public long MinFileFlagSize { get; set; }
        public DateTime MinFlagAccessTime { get; set; }
        public bool ScanAllDrives { get; set; }
        public bool ScanProgramFolders { get; set; }
        public bool ScanDownloadsFolder { get; set; }
        public bool ScanDocumentFolder { get; set; }
        public bool ScanPicturesFolder { get; set; }
        public bool ScanVideosFolder { get; set; }
        public bool ScanDesktopFolder { get; set; }
        public bool ScanMusicFolder { get; set; }
    }
}