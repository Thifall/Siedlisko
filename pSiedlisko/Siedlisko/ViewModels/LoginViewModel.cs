using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [Display(Description = "E-mail", Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public bool Create { get; set; }
    }
}
