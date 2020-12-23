using System;
using System.Collections.ObjectModel;

namespace FileCleanup.Models
{
    public class Configuration
    {
        public ObservableCollection<string> PathsNotToScan { get; set; } = new ObservableCollection<string>();
        public bool ScanSystemFolders { get; set; }
        public UserScanConfiguration CurrentUserSettings { get; set; }
        public UserScanConfiguration PublicUserSettings { get; set; }
        public bool ScanProgramFolders { get; set; }
        public bool ScanProgramDataFolders { get; set; }
        public long FlagFileSizeMin { get; set; }
        public long FlagFileSizeMax { get; set; }
        public DateTime FlagLastAccessDateMin { get; set; }
        public DateTime FlagLastAccessDateMax { get; set; }
    }
}