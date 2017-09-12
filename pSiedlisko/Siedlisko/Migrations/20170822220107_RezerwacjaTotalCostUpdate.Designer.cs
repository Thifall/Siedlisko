using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Siedlisko.Models;
using Siedlisko.Models.Enums;

namespace Siedlisko.Migrations
{
    [DbContext(typeof(SiedliskoContext))]
    [Migration("20170822220107_RezerwacjaTotalCostUpdate")]
    partial class RezerwacjaTotalCostUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Siedlisko.Models.Pokoj", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Pokoje");
                });

            modelBuilder.Entity("Siedlisko.Models.Price", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("PriceFor");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.ToTable("Prices");
                });

            modelBuilder.Entity("Siedlisko.Models.Rezerwacja", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<byte>("Adults");

                    b.Property<byte>("Children");

                    b.Property<DateTime>("EndDate");

                    b.Property<int?>("PokojId");

                    b.Property<string>("ReserverLastName");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("Status");

                    b.Property<decimal>("TotalCost");

                    b.HasKey("Id");

                    b.HasIndex("PokojId");

                    b.ToTable("Rezerwacje");
                });

            modelBuilder.Entity("Siedlisko.Models.Rezerwacja", b =>
                {
                    b.HasOne("Siedlisko.Models.Pokoj")
                        .WithMany("Reservations")
                        .HasForeignKey("PokojId");
                });
        }
    }
}
