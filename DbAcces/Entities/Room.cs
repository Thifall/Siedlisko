using DbAcces.Entities.Interfaces;
using System.Collections.Generic;

namespace DbAcces.Entities
{
    public class Room : IRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
