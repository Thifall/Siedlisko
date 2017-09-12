using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.ExtensionMethods
{
    public static class DateTimeExtensions
    {
        public static bool MonthStartsOn(this DateTime date, DayOfWeek dayOfWeek)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            return firstDayOfMonth.DayOfWeek == dayOfWeek;
        }

        public static DateTime LastDayOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));
        }
        
    }
}
