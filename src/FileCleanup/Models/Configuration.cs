using FileCleanup.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace FileCleanup.Models {
    public static class StringExtensions {
        public static bool IsIn(this string extension, List < string > extensions) {
            return extensions.Contains(extension);
        }

        public static bool IsIn(this string extension, string[] extensions) {
            foreach(var ext in extensions) {
                if (ext == extension) return true;
            }
            return false;
        }
    }

    public static class ConfigurationExtensions {
        public static bool IsOverFlagSize(this Configuration flagSize, long fileSize) {
            return (fileSize > flagSize.FlagFileSize) ? true : false;
        }

        public static bool IsOverFlagAccessDate(this Configuration flagDateTime, DateTime fileLastAccessDatetime) {
            return (fileLastAccessDatetime < flagDateTime.LastAccessFlagDate) ? true : false;
        }
    }

    public class BaseViewModel: INotifyPropertyChanged {

        public event PropertyChangedEventHandler PropertyChanged;
        internal void NotifyPropertyChanged(string propertyName) {
            PropertyChanged ? .Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

    public class Configuration {
        public ObservableCollection < string > PathsNotToScan {
            get;
            set;
        } = new ObservableCollection < string > ();
        public bool ScanSystemFolders {
            get;
            set;
        }
        public UserScanConfiguration CurrentUserSettings {
            get;
            set;
        }
        public UserScanConfiguration PublicUserSettings {
            get;
            set;
        }
        public bool ScanProgramFolders {
            get;
            set;
        }
        public bool ScanProgramDataFolders {
            get;
            set;
        }
        public long FlagFileSize {
            get;
            set;
        }
        public DateTime LastAccessFlagDate {
            get;
            set;
        }
    }

    public class UserScanConfiguration {
        public bool ScanDesktop {
            get;
            set;
        }
        public bool ScanDocuments {
            get;
            set;
        }
        public bool ScanDownloads {
            get;
            set;
        }
        public bool ScanMusic {
            get;
            set;
        }
        public bool ScanPictures {
            get;
            set;
        }
        public bool ScanVideos {
            get;
            set;
        }
    }

    public class ScanProgress {
        public bool IsFile {
            get;
            set;
        }
        public FileProps File {
            get;
            set;
        }
        public bool IsUpdateStatus {
            get;
            set;
        }
        public string CurrentDirectory {
            get;
            set;
        }

        public ScanProgress(FileProps file, bool isFile, bool isUpdateStatus = false) {
            File = file;
            IsFile = isFile;
            IsUpdateStatus = isUpdateStatus;
        }

        public ScanProgress(string currentDirectory = "", bool onlyUpdate = true) {
            CurrentDirectory = currentDirectory;
            IsUpdateStatus = onlyUpdate;
        }
    }


    public class FileProps: BaseViewModel {
        public string Name => Path.GetFileName(FullPath);

        private bool isScanable = true;
        public bool IsScanable {
            get {
                return isScanable;
            }
            set {
                isScanable = value;
                NotifyPropertyChanged(nameof(IsScanable));
                NotifyPropertyChanged(nameof(IsScanableStatus));
            }
        }

        public string IsScanableStatus => IsScanable ? "Don't Scan" : "Not Scanning";
        public string FullPath {
            get;
            set;
        }
        public FileType Type {
            get;
            set;
        }
        public DateTime LastAccessed {
            get;
            set;
        }
        public long ByteSize {
            get;
            set;
        }
        public long KiloByteSize => ByteSize / 1024;
        public string Size {
            get {
                var factor = 1024;
                var convertedSize = ByteSize;
                if (convertedSize < factor)
                    return $ "{convertedSize} Bytes";
                else
                    convertedSize /= factor;

                if (convertedSize < factor)
                    return $ "{convertedSize} Kb";
                else
                    convertedSize /= factor;

                if (convertedSize < factor)
                    return $ "{convertedSize} Mb";
                else
                    convertedSize /= factor;

                return $ "{convertedSize} Gb";
            }
        }
        public string DaysSinceViewed {
            get {
                var time = DateTime.Now.Subtract(LastAccessed);
                return $ "{time.Days} days";
            }
        }

        public FileProps(FileInfo file) {
            FullPath = file.FullName;
            LastAccessed = file.LastAccessTime;
            ByteSize = file.Length;
            Type = GetFileTypeFromExtension(file.Extension);
        }

        public FileProps(DirectoryInfo directory) {
            FullPath = directory.FullName;
            LastAccessed = directory.LastAccessTime;
            ByteSize = 0;
            Type = FileType.directory;
        }

        public FileType GetFileTypeFromExtension(string extension) {
            string[] pictureExtensions = {
                ".jpg",
                ".jpeg",
                ".img",
                ".png",
                ".gif",
                ".ai",
                ".bmp",
                ".ico",
                ".ps",
                ".psd",
                "svg",
                ".tiff",
                ".tif"
            };
            string[] movieExtensions = {
                ".3g2",
                ".3gp",
                ".avi",
                ".flv",
                ".h264",
                ".m4v",
                ".mkv",
                ".mov",
                ".mp4",
                ".mpg",
                ".mpeg",
                ".rm",
                ".swf",
                ".vob",
                ".wmv"
            };
            string[] musicExtensions = {
                ".mp3",
                ".aif",
                ".cda",
                ".mid",
                ".midi",
                ".mpa",
                ".ogg",
                ".wav",
                ".wma",
                ".wpl"
            };
            string[] documentExtensions = {
                ".doc",
                ".docx",
                ".odt",
                ".pdf",
                ".rtf",
                ".tex",
                ".txt",
                ".wpd"
            };
            string[] logExtensions = {
                ".log"
            };

            //Newly added categories
            string[] compressedExtensions = {
                ".7z",
                ".arj",
                ".deb",
                ".pkg",
                ".rar",
                ".rpm",
                ".tar.gz",
                ".z",
                ".zip"
            };
            string[] mediaExtensions = {
                ".bin",
                ".dmg",
                ".iso",
                ".toast",
                ".vcd"
            };
            string[] dataExtensions = {
                ".csv",
                ".dat",
                ".db",
                ".dbf",
                ".mdb",
                ".sav",
                ".sql",
                ".tar",
                ".xml"
            };
            string[] emailExtensions = {
                ".email",
                ".eml",
                ".emlx",
                ".msg",
                ".oft",
                ".ost",
                ".pst",
                ".vcf"
            };
            string[] executableExtensions = {
                ".apk",
                ".bat",
                ".bin",
                ".cgi",
                ".pl",
                ".com",
                ".exe",
                ".gadget",
                ".jar",
                ".msi",
                ".py",
                ".wsf"
            };

            string[] fontExtensions = {
                ".fnt",
                ".fon",
                ".otf",
                ".ttf"
            };

            string[] internetExtensions = {
                ".asp",
                ".aspx",
                ".cer",
                ".cfm",
                ".cgi",
                ".pl",
                ".css",
                ".htm",
                ".html",
                ".js",
                ".jsp",
                ".part",
                ".php",
                ".py",
                ".rss",
                ".xhtml"
            };

            string[] presentationExtensions = {
                ".key",
                ".odp",
                ".pps",
                ".ppt",
                ".pptx"
            };

            string[] codeExtensions = {
                ".c",
                ".cgi",
                ".pl",
                ".class",
                ".cpp",
                ".cs",
                ".h",
                ".java",
                ".php",
                ".py",
                ".sh",
                ".swift",
                ".vb"
            };

            string[] spreadsheetExtensions = {
                ".ods",
                ".xls",
                ".xlsm",
                ".xlsx"
            };

            string[] systemExtensions = {
                ".bak",
                ".cab",
                ".cfg",
                ".cpl",
                ".cur",
                ".dll",
                ".dmp",
                ".drv",
                ".icns",
                ".ico",
                ".ini",
                ".lnk",
                ".msi",
                ".sys",
                ".tmp"
            };

            if (extension.IsIn(pictureExtensions))
                return FileType.picture;
            else if (extension.IsIn(movieExtensions))
                return FileType.movie;
            else if (extension.IsIn(musicExtensions))
                return FileType.music;
            else if (extension.IsIn(documentExtensions))
                return FileType.document;
            else if (extension.IsIn(logExtensions))
                return FileType.log;
            else
                return FileType.unknown;
        }
    }

    public enum FileType {
        unknown,
        directory,
        picture,
        movie,
        document,
        music,
        log
    }
}