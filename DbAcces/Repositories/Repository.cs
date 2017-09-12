using DbAcces.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DbAcces.Entities;
using DbAcces.DbContext;
using Microsoft.EntityFrameworkCore;

namespace DbAcces.Repositories
{
    public class Repository : IRepository
    {
        #region Fields and Properties
        private SiedliskoContext _context;
        #endregion

        #region ctor
        public Repository(SiedliskoContext context)
        {
            _context = context;
        }
        #endregion

        #region Reservations
        public async Task<bool> AddReservation(Reservation reservation, Room room)
        {
            GetRoom(room.Id).Reservations.Add(reservation);
            return await _context.SaveChangesAsync() > 0;
        }

        public IEnumerable<Reservation> GetAllReservations()
        {
            return _context.Rezerwacje;
        }

        public Reservation GetReservationById(int id)
        {
            return _context.Rezerwacje.ToList().SingleOrDefault(x => x.Id == id);
        }

        public async Task<bool> RemoveReservation(Reservation reservation)
        {
            var removedRes = _context.Rezerwacje.Remove(reservation);
            bool success;
            if (removedRes.State == Microsoft.EntityFrameworkCore.EntityState.Deleted)
            {
                success = true;
            }
            else
            {
                success = false;
            }
            await _context.SaveChangesAsync();
            return success;

        }

        public async Task<bool> UpdateReservation(Action<Reservation> action, int Id)
        {
            action.Invoke(GetReservationById(Id));
            return (await _context.SaveChangesAsync() == 1);
        }
        #endregion

        #region Rooms
        public Room GetRoom(int id)
        {
            return GetRooms().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Returns a Room object which contains specific reservation
        /// </summary>
        /// <param name="reservation"></param>
        /// <returns></returns>
        public Room GetRoom(Reservation reservation)
        {
            return GetRooms().FirstOrDefault(x => x.Reservations.Contains(reservation));
        }

        public int GetRoomIdOfReservation(Reservation reservation)
        {
            var roomId = GetRooms().FirstOrDefault(r => r.Reservations.Contains(reservation)).Id;
            return roomId;
        }

        public IEnumerable<Reservation> GetRoomReservations(Room room)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Room> GetRooms()
        {
            return _context.Pokoje
                .Include(x => x.Reservations)
                .ToList();
        }

        #endregion
    }
}
