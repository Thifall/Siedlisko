﻿using Siedlisko.Models.Helper;
using Siedlisko.Models.Interfaces;
using SiedliskoCommon.Models.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Siedlisko.Models
{
    public class Reservation : IReservation
    {
        #region Fields and Properties
        [Key]
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
        public string StatusString
        {
            get
            {
                return NamingHelper.GetReservationStatusString(Status);
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
