using Siedlisko.Models;
using Siedlisko.Models.Interfaces;
using Siedlisko.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.ViewModels
{
    public class IndexPartielViewModel : IViewModel
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

        public bool OperationSucces { get; set; }
        public string OperationResultsDescription { get; set; }
    }
}
