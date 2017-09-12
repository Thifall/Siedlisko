using DbAcces.DbContext;
using DbAcces.Entities;
using DbAcces.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Siedlisko.Models.Helper
{
    public class SiedliskoDataSeeder
    {
        #region Fields and Properties
        private SiedliskoContext _context;
        private UserManager<SiedliskoUser> _userManager;
        private IConfigurationRoot _config;
        #endregion

        #region .ctor
        public SiedliskoDataSeeder(SiedliskoContext dbContext, UserManager<SiedliskoUser> userManager, IConfigurationRoot config)
        {
            _context = dbContext;
            _userManager = userManager;
            _config = config;
        }
        #endregion

        #region Public API
        public async Task SeedData()
        {

            if (await _userManager.FindByEmailAsync("migosh@o2.pl") == null)
            {
                var user = new SiedliskoUser()
                {
                    Nazwisko = _config["DevUserDetails:DevSurname"],
                    Imie = _config["DevUserDetails:DevName"],
                    Email = _config["DevUserDetails:DevEmail"],
                    UserName = "jmusiol",
                };
                user.Roles.Add(new IdentityUserRole<string>() { RoleId = _config["DevUserDetails:DevRoleId"] });

                var devUserCreationResults = await _userManager.CreateAsync(user, _config["DevUserDetails:DevPassword"]);
            }

            if (!_context.Pokoje.Any() && !_context.Prices.Any() && !_context.Rezerwacje.Any())
            {
                _context.Prices.Add(new Price() { PriceFor = PriceFor.Adult, Value = 10 });
                _context.Prices.Add(new Price() { PriceFor = PriceFor.Room, Value = 140 });

                for (int i = 0; i < 4; i++)
                {
                    var pokoj = new Room()
                    {
                        Name = $"Domek {i + 1}",
                        Reservations = new List<Reservation>()
                        {
                            new Reservation()
                            {
                                ReserverLastName = "Musioł",
                                StartDate = DateTime.Now,
                                EndDate = DateTime.Now.AddDays(4),
                                Adults = 5,
                                Children = 2,
                                Status = ReservationStatus.WaitingForConfirmation,
                            }
                        }
                    };
                    _context.Pokoje.Add(pokoj);
                    _context.Rezerwacje.AddRange(pokoj.Reservations);
                }
                await _context.SaveChangesAsync();
            }
        }
        #endregion
    }
}
