using FileCleanup.Models;
using System;


namespace FileCleanup.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool IsOverFlagSize(this Configuration flagSize, long fileSize)
        {
            return (fileSize > flagSize.FlagFileSize) ? true : false;
        }

        public static bool IsOverFlagAccessDate(this Configuration flagDateTime, DateTime fileLastAccessDatetime)
        {
            return (fileLastAccessDatetime < flagDateTime.LastAccessFlagDate) ? true : false;
        }
    }
}
