using FileCleanup.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileCleanup.Helpers
{
    public static class Utils
    {
        public static FileType GetFileType(this string extension)
        {
            if (extension.Equals(".jpg"))
            {
                return FileType.movie;
            }
            else if (extension.Equals(""))
            {
                return FileType.document;
            }
            else
            {
                return FileType.picture;
            }
        }

    }
}
