using System;
using Siedlisko.Models.Enums;

namespace Siedlisko.Models.Interfaces
{
    public interface IReservation
    {
        byte Adults { get; set; }
        byte Children { get; set; }
        DateTime EndDate { get; set; }
        int NumberOfAccomodations { get; }
        byte PeopleQuantity { get; }
        DateTime ReservedOn { get; set; }
        string ReserverLastName { get; set; }
        string ReserverUserName { get; set; }
        DateTime StartDate { get; set; }
        ReservationStatus Status { get; set; }
        decimal TotalCost { get; set; }

        string MonthString();
    }
}