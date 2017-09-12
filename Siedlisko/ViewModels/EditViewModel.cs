using DbAcces.Entities;
using Siedlisko.ViewModels.Interfaces;

namespace Siedlisko.ViewModels
{
    public class EditViewModel : IViewModel
    {
        public Reservation Reservation { get; set; }
        public bool OperationSucces { get; set; }
        public string OperationResultsDescription { get; set; }
    }
}
