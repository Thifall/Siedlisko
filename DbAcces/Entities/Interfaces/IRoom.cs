using System.Collections.Generic;

namespace DbAcces.Entities.Interfaces
{
    public interface IRoom
    {
        int Id { get; set; }
        string Name { get; set; }
        ICollection<Reservation> Reservations { get; set; }
    }
}