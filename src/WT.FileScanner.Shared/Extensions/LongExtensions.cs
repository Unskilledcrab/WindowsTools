using System;
using System.Collections.Generic;
using WT.FileScanner.Shared.Models;

namespace WT.FileScanner.Shared.Extensions
{
    public static class LongExtensions
    {
        private readonly static Dictionary<FileSizeType, long> fileSizeMultipliers = new Dictionary<FileSizeType, long>
        {
            { FileSizeType.Kb, 1024 },
            { FileSizeType.Mb, (long)Math.Pow(1024, 2) },
            { FileSizeType.Gb, (long)Math.Pow(1024, 3) }
        };

        public static long ConvertFileSizeToBytes(this long size, FileSizeType fileSizeType)
        {
            return fileSizeMultipliers.ContainsKey(fileSizeType) ? size * fileSizeMultipliers[fileSizeType] : size;
        }

        public static long ConvertSizeToBytes(this int size, FileSizeType fileSizeType)
        {
            return fileSizeMultipliers.ContainsKey(fileSizeType) ? size * fileSizeMultipliers[fileSizeType] : size;
        }
    }
}