using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Siedlisko.Models.Interfaces;
using SiedliskoCommon.Models;

namespace Siedlisko.Models
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
        public DbSet<EmailMessage> EmailMessages { get; set; }
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

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Room>().HasIndex(r => r.Name).IsUnique();
            builder.Entity<Reservation>().Ignore(x => x.StatusString);
            base.OnModelCreating(builder);
        }
        #endregion
    }
}
