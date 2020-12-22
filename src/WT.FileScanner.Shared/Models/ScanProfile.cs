using System.Collections.Generic;

namespace WT.FileScanner.Shared.Models
{
    public class ScanProfile : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Settings Settings { get; set; }
        public ICollection<IgnoredPath> IgnoredPaths { get; set; }
    }
}