using DbAcces.Entities;
using DbAcces.Repositories.Interfaces;

namespace Siedlisko.ViewModels
{
    public class IndexPartielViewModel
    {
        private IRepository _repository;

        public IndexPartielViewModel(Reservation reservation, IRepository repository)
        {
            _repository = repository;
            Reservation = reservation;
        }

        public Reservation Reservation { get; set; }
        public int RoomId {
            get
            {
                return _repository.GetRoomIdOfReservation(Reservation);
            }
        }
    }
}
