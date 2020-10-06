using FileCleanup.Models;

namespace FileCleanup.ProgressModels
{
    public class ScanProgress
    {
        public bool IsFile { get; set; }
        public FileProps File { get; set; }
        public bool IsUpdateStatus { get; set; }
        public string CurrentDirectory { get; set; }

        public ScanProgress(FileProps file, bool isFile, bool isUpdateStatus = false)
        {
            File = file;
            IsFile = isFile;
            IsUpdateStatus = isUpdateStatus;
        }

        public ScanProgress(string currentDirectory = "", bool onlyUpdate = true)
        {
            CurrentDirectory = currentDirectory;
            IsUpdateStatus = onlyUpdate;
        }
    }
}
