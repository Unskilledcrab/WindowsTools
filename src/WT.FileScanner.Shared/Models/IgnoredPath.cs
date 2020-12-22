namespace WT.FileScanner.Shared.Models
{
    public class IgnoredPath : BaseEntity
    {
        public ScanProfile ScanProfile { get; set; }
        public string Path { get; set; }
    }
}