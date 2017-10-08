using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models.Interfaces
{
    public interface IRepository
    {

        #region Reservations
        IEnumerable<Reservation> GetAllReservations();
        IEnumerable<Reservation> GetRoomReservations(Room room);
        Reservation GetReservationById(int id);
        Task<bool> RemoveReservation(Reservation reservation);
        Task<Reservation> AddReservation(Reservation reservation, Room room);
        Task<bool> UpdateReservation(Action<Reservation> action, int Id);
        #endregion

        #region Rooms
        Room GetRoom(int id);
        Room GetRoom(Reservation reservation);
        IEnumerable<Room> GetRooms();
        int GetRoomIdOfReservation(Reservation reservation);
        #endregion
    }
}
