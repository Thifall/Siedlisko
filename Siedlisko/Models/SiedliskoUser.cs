using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models
{
    public class SiedliskoUser : IdentityUser
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
    }
}
