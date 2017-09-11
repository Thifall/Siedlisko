using Siedlisko.Models.Interfaces;
using Siedlisko.ViewModels;

namespace Siedlisko.Controllers.Interfaces
{
    public interface IReservationController
    {
        bool CheckIfReservationCanBeMade(IRoom room, CreateReservationViewModel createVM);
    }
}