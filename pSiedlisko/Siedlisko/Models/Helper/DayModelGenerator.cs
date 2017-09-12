using Siedlisko.ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models.Helper
{
    public static class DayModelGenerator
    {
        #region Public API
        public static IEnumerable<DayModel> GetDaysOfMonth(DateTime date)
        {
            return GetDaysOfMonth(date.Year, date.Month);
        }
        #endregion

        #region Private Methods
        private static IEnumerable<DayModel> GetDaysOfMonth(int year, int month)
        {
            List<DayModel> daysOfMonth = new List<DayModel>();
            for (int i = 1; i <= DateTime.DaysInMonth(year, month); i++)
            {
                daysOfMonth.Add(new DayModel(year, month, i, true));
            }
            daysOfMonth.AddRange(GetLeadingAndTrailingFillerDays(daysOfMonth[0].Date));
            return daysOfMonth;
        }

        private static IEnumerable<DayModel> GetLeadingAndTrailingFillerDays(DateTime date)
        {
            //getting full week of month start
            var day = new DateTime(date.Year, date.Month, 1);
            var fillingDays = new List<DayModel>();

            if (day.DayOfWeek != DayOfWeek.Monday)
            {
                while (day.DayOfWeek != DayOfWeek.Monday)
                {
                    day = day.AddDays(-1);
                    fillingDays.Add(new DayModel(day, false));
                }
            }

            //getting full week of month end
            day = date.LastDayOfMonth();
            if (day.DayOfWeek != DayOfWeek.Sunday)
            {
                day = day.AddDays(1);
                while (day.DayOfWeek != DayOfWeek.Sunday)
                {
                    fillingDays.Add(new DayModel(day, false));
                    day = day.AddDays(1);
                }
                fillingDays.Add(new DayModel(day, false));
            }
            return fillingDays;
        } 
        #endregion
    }
}
