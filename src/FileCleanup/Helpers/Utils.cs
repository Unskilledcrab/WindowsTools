using FileCleanup.Extensions;
using FileCleanup.Models;
using System;

namespace FileCleanup.Helpers
{
    public static class Utils
    {
        public static FileType GetFileTypeFromExtension(string extension)
        {
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
            else if (extension.IsIn(compressedExtensions))
                return FileType.compressed;
            else if (extension.IsIn(mediaExtensions))
                return FileType.media;
            else if (extension.IsIn(dataExtensions))
                return FileType.data;
            else if (extension.IsIn(emailExtensions))
                return FileType.email;
            else if (extension.IsIn(executableExtensions))
                return FileType.executable;
            else if (extension.IsIn(fontExtensions))
                return FileType.font;
            else if (extension.IsIn(internetExtensions))
                return FileType.internet;
            else if (extension.IsIn(presentationExtensions))
                return FileType.presentation;
            else if (extension.IsIn(codeExtensions))
                return FileType.code;
            else if (extension.IsIn(spreadsheetExtensions))
                return FileType.spreadsheet;
            else if (extension.IsIn(systemExtensions))
                return FileType.system;
            else
                return FileType.unknown;
        }

        public static long ConvertSizeToByte(long size, FileSizeType type)
        {
            switch (type)
            {
                case FileSizeType.Kb:
                    size *= 1024;
                    break;
                case FileSizeType.Mb:
                    size *= (long)Math.Pow(1024, 2);
                    break;
                case FileSizeType.Gb:
                    size *= (long)Math.Pow(1024, 3);
                    break;
                default:
                    size *= (long)Math.Pow(1024, 4);
                    break;
            }
            return size;
        }

        public static FileSizeType ConvertStringToSizeType(string sizeType)
        {
            Enum.TryParse(sizeType, out FileSizeType result);
            return result;
        }
    }
}
