using FileCleanup.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Text;

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
        public long FlagFileSize { get; set; }
        public DateTime LastAccessFlagDate { get; set; }
    }
}
