using System;
using System.Globalization;

namespace Seats4Me.Data.Common
{
    public static class DateTimeExtensions
    {
        public static int Week(this DateTime day)
        {
            return CultureInfo.CurrentCulture.Calendar
                .GetWeekOfYear(day,
                    CalendarWeekRule.FirstFourDayWeek,
                    DayOfWeek.Monday
                );
        }
        public static DateTime FirstDayOfWeek(int week, int year)
        {
            var firstDay = new DateTime(year, 1, 1);
            var addWeeks = week + (firstDay.Week() == 1 ? -1: 0);
            firstDay = firstDay.AddDays(-(int) firstDay.DayOfWeek);
            return firstDay.AddDays(addWeeks * 7);
        }
    }
}
