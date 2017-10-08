using SiedliskoCommon.Models.Enums;
using System;

namespace Siedlisko.Models
{
    public class DayModel
    {
        #region Fields and Properties
        public DateTime Date { get; }
        public bool IsRegularMonthDay { get; }
        public ReservationStatus ReservationState { get; set; }
        public int ReservationCounter { get; set; }
        #endregion

        #region .ctor
        public DayModel(DateTime date, bool isRegularMonthDay)
        {
            ReservationCounter = 0;
            this.Date = date;
            this.IsRegularMonthDay = isRegularMonthDay;
            ReservationState = ReservationStatus.Free;
        }

        public DayModel(int year, int month, int day, bool isRegularMonthDay) : this(new DateTime(year, month, day), isRegularMonthDay)
        {

        } 
        #endregion
    }
}
