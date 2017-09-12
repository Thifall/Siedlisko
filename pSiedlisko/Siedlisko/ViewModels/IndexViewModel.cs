using Siedlisko.ExtensionMethods;
using Siedlisko.Models;
using Siedlisko.Models.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.ViewModels
{
    public class IndexViewModel
    {
        #region Fields and Properties
        public DateTime Date { get; set; }
        private List<DayModel> Days = new List<DayModel>();

        #endregion

        #region ctor
        public IndexViewModel()
        {
            Date = DateTime.Now;
        }
        #endregion

        #region Public API
        public IEnumerable<DayModel> GetAllDaysOfWeek(DayOfWeek dayOfWeek)
        {
            return Days.Where(x => x.Date.DayOfWeek == dayOfWeek).OrderBy(x => x.Date);
        }

        public void PrepareDaysStatuses(IEnumerable<Reservation> rezerwacje)
        {
            Days = DayModelGenerator.GetDaysOfMonth(Date).ToList();
            foreach (var res in rezerwacje)
            {
                foreach (var day in Days.Where(x => (x.Date >= res.StartDate) && (x.Date <= res.EndDate)))
                {
                    day.ReservationCounter++;
                    if(day.ReservationCounter > 3)
                    {
                        day.ReservationState = Models.Enums.ReservationStatus.Full;
                    }
                    else if (day.ReservationCounter > 2)
                    {
                        day.ReservationState = Models.Enums.ReservationStatus.LastSpots;
                    }
                    else if (day.ReservationCounter > 0)
                    {
                        day.ReservationState = Models.Enums.ReservationStatus.PartiallyTaken;
                    }
                }
            }
        }

        public bool MonthStartsOn(DayOfWeek dayOfWeek)
        {
            return Date.MonthStartsOn(dayOfWeek);
        }
        
        public bool IsADaySpecificDayOfWeek(int day, DayOfWeek dayOfWeek)
        {
            var testedDate = new DateTime(Date.Year, Date.Month, day);
            return testedDate.DayOfWeek == dayOfWeek;
        } 

        public string GetMonthsString()
        {
            return NamingHelper.GetMonthString(Date.Month);
        }
        #endregion
    }
}
