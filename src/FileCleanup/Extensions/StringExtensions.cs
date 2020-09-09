using System.Collections.Generic;

namespace FileCleanup.Extensions
{
    public static class StringExtensions
    {
        public static bool IsIn(this string extension, List<string> extensions)
        {
            return extensions.Contains(extension);
        }

        public static bool IsIn(this string extension, string[] extensions)
        {
            foreach (var ext in extensions)
            {
                if (ext == extension) return true;
            }
            return false;
        }
    }
}
