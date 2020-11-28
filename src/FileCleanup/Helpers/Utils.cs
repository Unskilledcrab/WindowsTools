using FileCleanup.Extensions;
using FileCleanup.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FileCleanup.Helpers
{
    public static class Utils
    {
        private readonly static Dictionary<FileType, string[]> suportedFileExtensions = new()
        {
            { FileType.picture, new string[] { ".jpg", ".jpeg", ".img", ".png", ".gif", ".ai", ".bmp", ".ico", ".ps", ".psd", "svg", ".tiff", ".tif" } },
            { FileType.movie, new string[] { ".3g2", ".3gp", ".avi", ".flv", ".h264", ".m4v", ".mkv", ".mov", ".mp4", ".mpg", ".mpeg", ".rm", ".swf", ".vob", ".wmv" } },
            { FileType.music, new string[] { ".mp3", ".aif", ".cda", ".mid", ".midi", ".mpa", ".ogg", ".wav", ".wma", ".wpl" } },
            { FileType.document, new string[] { ".doc", ".docx", ".odt", ".pdf", ".rtf", ".tex", ".txt", ".wpd" } },
            { FileType.log, new string[] { ".log" } },
            { FileType.compressed, new string[] { ".7z", ".arj", ".deb", ".pkg", ".rar", ".rpm", ".tar.gz", ".z", ".zip" } },
            { FileType.media, new string[] { ".bin", ".dmg", ".iso", ".toast", ".vcd" } },
            { FileType.data, new string[] { ".csv", ".dat", ".db", ".dbf", ".mdb", ".sav", ".sql", ".tar", ".xml" } },
            { FileType.email, new string[] { ".email", ".eml", ".emlx", ".msg", ".oft", ".ost", ".pst", ".vcf" } },
            { FileType.executable, new string[] { ".apk", ".bat", ".bin", ".cgi", ".pl", ".com", ".exe", ".gadget", ".jar", ".msi", ".py", ".wsf" } },
            { FileType.font, new string[] { ".fnt", ".fon", ".otf", ".ttf" } },
            { FileType.internet, new string[] { ".asp", ".aspx", ".cer", ".cfm", ".cgi", ".pl", ".css", ".htm", ".html", ".js", ".jsp", ".part", ".php", ".py", ".rss", ".xhtml" } },
            { FileType.presentation, new string[] { ".key", ".odp", ".pps", ".ppt", ".pptx" } },
            { FileType.code, new string[] { ".c", ".cgi", ".pl", ".class", ".cpp", ".cs", ".h", ".java", ".php", ".py", ".sh", ".swift", ".vb" } },
            { FileType.spreadsheet, new string[] { ".ods", ".xls", ".xlsm", ".xlsx" } },
            { FileType.system, new string[] { ".bak", ".cab", ".cfg", ".cpl", ".cur", ".dll", ".dmp", ".drv", ".icns", ".ico", ".ini", ".lnk", ".msi", ".sys", ".tmp" } }
        };

        private readonly static Dictionary<FileSizeType, long> fileSizeMultipliers = new()
        {
            { FileSizeType.Kb, 1024 },
            { FileSizeType.Mb, (long)Math.Pow(1024, 2) },
            { FileSizeType.Gb, (long)Math.Pow(1024, 3) }
        };

        public static FileType GetFileTypeFromExtension(string extension)
        {
            foreach (var item in suportedFileExtensions)
            {
                if (item.Value.Contains(extension))
                    return item.Key;
            }

            return FileType.unknown;
        }

        public static long ConvertSizeToByte(long size, FileSizeType type)
        {
            return fileSizeMultipliers.ContainsKey(type) ? size * fileSizeMultipliers[type] : size * (long)Math.Pow(1024, 4);
        }

        public static FileSizeType ConvertStringToSizeType(string sizeType)
        {
            Enum.TryParse(sizeType, out FileSizeType result);
            return result;
        }
    }
}