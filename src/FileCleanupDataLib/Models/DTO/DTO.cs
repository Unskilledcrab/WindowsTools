using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileCleanupDataLib.Models
{
    public class CfgScanProfile
    {
        [Key]
        public int Id { get; set; }
        public string ProfileName { get; set; }
        public string Description { get; set; }
        //One-to-many relationship.
        public ICollection<CfgIgnoredPath> IgnoredPaths { get; set; }

        [ForeignKey(nameof(CfgSettings))]
        public int CfgSettingsId { get; set; }
        public CfgSettings CfgSettings { get; set; }
    }

    public class CfgIgnoredPath
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(ScanProfile))]
        public int ScanProfileId { get; set; }
        public CfgScanProfile ScanProfile { get; set; }
        public string Path { get; set; }
    }

    public class CfgSettings
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey(nameof(ScanProfile))]
        public int ScanProfileId { get; set; }
        public CfgScanProfile ScanProfile { get; set; }
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
