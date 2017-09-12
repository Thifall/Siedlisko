using DbAcces.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DbAcces.DbContext
{
    public class SiedliskoContext : IdentityDbContext<SiedliskoUser>
    {
        #region Fields and Properties
        private IConfigurationRoot _config;
        #endregion

        #region DbSets
        public DbSet<Room> Pokoje { get; set; }
        public DbSet<Reservation> Rezerwacje { get; set; }
        public DbSet<Price> Prices { get; set; }
        #endregion

        #region ctor
        public SiedliskoContext(IConfigurationRoot config, DbContextOptions options) : base(options)
        {
            _config = config;
        }
        #endregion

        #region Private Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            optionsBuilder.UseSqlServer(_config["ConnectionStrings:SqlServerConnectionString"]);
        }
        #endregion
    }
}
