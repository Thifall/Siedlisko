using DbAcces.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DbAcces.DbContext
{
    public class SiedliskoContext : IdentityDbContext<SiedliskoUser>
    {
        #region DbSets
        public DbSet<Room> Pokoje { get; set; }
        public DbSet<Reservation> Rezerwacje { get; set; }
        public DbSet<Price> Prices { get; set; }
        #endregion

        #region ctor
        public SiedliskoContext(DbContextOptions options) : base(options)
        {
        }
        #endregion
    }
}
