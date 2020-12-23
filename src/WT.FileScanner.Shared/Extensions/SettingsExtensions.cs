using System;
using WT.FileScanner.Shared.Models;

namespace WT.FileScanner.Shared.Extensions
{
    public static class SettingsExtensions
    {
        public static bool IsFlagSize(this Settings source, long fileSize)
        {
            return (fileSize > source.FlagFileSizeMin && fileSize < source.FlagFileSizeMax);
        }

        public static bool IsFlagAccessDate(this Settings source, DateTime fileLastAccessDatetime)
        {
            return (fileLastAccessDatetime > source.FlagAccessDateTimeMin && fileLastAccessDatetime < source.FlagAccessDateTimeMax);
        }
    }
}