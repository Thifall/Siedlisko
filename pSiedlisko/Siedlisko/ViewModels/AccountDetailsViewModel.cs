using Siedlisko.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.ViewModels
{
    public class AccountDetailsViewModel : IViewModel
    {
        [Display(Name = "Nazwa użytkownika")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [MinLength(3)]
        public string Nazwisko { get; set; }

        [Display(Name = "Imię")]
        [Required(ErrorMessage = "Imię jest wymagane")]
        [MinLength(3)]
        public string Imie { get; set; }

        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "Numer kontaktowy")]
        [Required (ErrorMessage = "Numer kontaktowy jest wymagany")]
        [MinLength(9, ErrorMessage = "To nie jest prawidłowy numer telefonu")]
        [Phone(ErrorMessage = "To nie jest prawidłowy numer telefonu")]
        public string PhoneNumber { get; set; }

        public bool ChangesSaved { get; set; }
        public string ChangeNotification { get; set; }

        public bool OperationSucces { get; set; }
        public string OperationResultsDescription { get; set; }
    }
}
