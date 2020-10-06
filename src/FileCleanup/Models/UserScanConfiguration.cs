namespace FileCleanup.Models
{
    public class UserScanConfiguration
    {
        public bool ScanDesktop { get; set; }
        public bool ScanDocuments { get; set; }
        public bool ScanDownloads { get; set; }
        public bool ScanMusic { get; set; }
        public bool ScanPictures { get; set; }
        public bool ScanVideos { get; set; }
    }
}
