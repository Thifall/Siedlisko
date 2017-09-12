using DbAcces.Entities;
using DbAcces.Repositories.Interfaces;
using Siedlisko.ViewModels.Interfaces;
using System.Collections.Generic;

namespace Siedlisko.ViewModels
{
    public class ReservationIndexViewModel : IViewModel
    {
        private IRepository _repository;
        public IEnumerable<Reservation> Reservations { get; set; }
        public IRepository repo { get { return _repository; } }

        public ReservationIndexViewModel(IEnumerable<Reservation> reservations, IRepository repository)
        {
            _repository = repository;
            Reservations = reservations;
        }

        public bool OperationSucces { get; set; }
        public string OperationResultsDescription { get; set; }

    }
}
