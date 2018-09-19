namespace Seats4Me.API.Common
{
    public static class StringExtensions
    {
        public static string InitCap(this string lower)
        {
            if (string.IsNullOrEmpty(lower))
            {
                return lower;
            }

            return char.ToUpper(lower[0]) + lower.Substring(1, lower.Length - 1);
        }
    }
}