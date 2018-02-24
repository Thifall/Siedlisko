using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Siedlisko.Models;
using Siedlisko.ViewModels.Interfaces;

namespace Siedlisko.ViewModels
{
    public class RegisterViewModel : IViewModel
    {
        [Display(Name = "Nazwa użytkownika*")]
		[Required(ErrorMessage = "Nazwa użytkownika jest wymagana")]
		[MinLength(3)]
        public string UserName { get; set; }

		[Display(Name = "Nazwisko*")]
        [Required(ErrorMessage = "Nazwisko jest wymagane")]
        [MinLength(3)]
        public string Nazwisko { get; set; }

        [Display(Name = "Imię*")]
        [Required(ErrorMessage = "Imię jest wymagane")]
        [MinLength(3)]
        public string Imie { get; set; }

        [Display(Name = "E-mail*")]
		[Required(ErrorMessage = "Adres e-mail jest wymagany")]
		[EmailAddress(ErrorMessage = "Nieprawidłowy adres e-mail")]
        public string Email { get; set; }

        [Display(Name = "Numer kontaktowy*")]
        [Required(ErrorMessage = "Numer kontaktowy jest wymagany")]
        [Phone(ErrorMessage = "To nie jest prawidłowy numer telefonu")]
        public string PhoneNumber { get; set; }

		[Required(ErrorMessage = "Hasło wymagane")]
        [Display(Name = "Hasło*")]
        [MinLength(8, ErrorMessage = "Za krótkie hasło (min. 8 znaków)")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Pole wymagane")]
		[Display(Name = "Powtórz hasło*")]
		[Compare("Password", ErrorMessage = "Hasła niezgodne")]
        public string PasswordRepeat { get; set; }

        public bool Succesed { get; set; }
        public string ErrorMsg { get; set; }
        public bool OperationSucces { get; set; }
        public string OperationResultsDescription { get; set; }
    }
}
