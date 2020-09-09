using FileCleanup.Extensions;
using FileCleanup.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCleanup.Helpers
{
    public static class Utils
    {
        public static FileType GetFileTypeFromExtension(string extension)
        {
            string[] pictureExtensions = { ".jpg", ".jpeg", ".img", ".png", ".gif" };
            string[] movieExtensions = { ".mov", ".mp4" };
            string[] musicExtensions = { ".mp3" };
            string[] documentExtensions = { ".doc", ".pdf" };
            string[] logExtensions = { ".log" };

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
