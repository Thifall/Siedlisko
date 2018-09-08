using Microsoft.AspNetCore.Identity;

namespace Siedlisko.Models
{
    public class SiedliskoUser : IdentityUser
    {
        public string Imie { get; set; }
        public string Nazwisko { get; set; }
    }
}
