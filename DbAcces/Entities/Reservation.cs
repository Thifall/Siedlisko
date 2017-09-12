using DbAcces.Entities.Interfaces;
using DbAcces.Enums;
using DbAcces.Helpers;
using System;

namespace DbAcces.Entities
{
    public class Reservation : IReservation
    {
        #region MyRegion
        public int Id { get; set; }
        public string ReserverLastName { get; set; }
        public string ReserverUserName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime ReservedOn { get; set; }
        public ReservationStatus Status { get; set; }
        public byte Adults { get; set; }
        public byte Children { get; set; }
        public int NumberOfAccomodations
        {
            get
            {
                return EndDate.Subtract(StartDate).Days;
            }
        }
        public decimal TotalCost { get; set; }
        public byte PeopleQuantity
        {
            get
            {
                return (byte)(Adults + Children);
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// returns name of Month that reservations begins within
        /// </summary>
        public string MonthString()
        {
            return NamingHelper.GetMonthString(StartDate.Month);
        }
        #endregion
    }
}
