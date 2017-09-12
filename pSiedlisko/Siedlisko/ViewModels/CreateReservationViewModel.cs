using Siedlisko.Models.DataValidators;
using Siedlisko.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.ViewModels
{
    public class CreateReservationViewModel : IViewModel
    {
        [DisplayName("Początek pobytu")]
        [PresentOrFutureDate(ErrorMessage = "Nieprawidłowa data")]
        [Required(ErrorMessage = "To pole musi zostać wypełnione")]
        public DateTime StartDate { get; set; }

        [DisplayName("Koniec pobytu")]
        [Required(ErrorMessage = "To pole musi zostać wypełnione")]
        [DateAheadValidator("StartDate", ErrorMessage = "Nieprawidłowa data")]
        [PresentOrFutureDate(ErrorMessage = "Nieprawidłowa data")]
        public DateTime EndDate { get; set; }

        [Required]
        [DisplayName("Numer domku")]
        public int RoomId { get; set; }

        [Required]
        [DisplayName("Dorośli")]
        public byte Adults { get; set; }

        [Required]
        [DisplayName("Dzieci")]
        public byte Children { get; set; }

        public bool OperationSucces { get; set; }
        public string OperationResultsDescription { get; set; }
    }
}
