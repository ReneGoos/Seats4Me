using System;

namespace Seats4Me.Data.Common
{
    public static class StringExtensions
    {
        public static string InitCap(this string lower)
        {
            if (string.IsNullOrEmpty(lower))
                return lower;
            return Char.ToUpper(lower[0]) + lower.Substring(1, lower.Length - 1);
        }
    }
}
