﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Siedlisko.Models;

namespace Siedlisko.Migrations
{
    [DbContext(typeof(SiedliskoContext))]
    [Migration("20170806192707_InitialMigration")]
    partial class InitialMigration
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

                    b.Property<int?>("RezerwacjaId");

                    b.Property<decimal>("Value");

                    b.HasKey("Id");

                    b.HasIndex("RezerwacjaId");

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

                    b.HasKey("Id");

                    b.HasIndex("PokojId");

                    b.ToTable("Rezerwacje");
                });

            modelBuilder.Entity("Siedlisko.Models.Price", b =>
                {
                    b.HasOne("Siedlisko.Models.Rezerwacja")
                        .WithMany("Prices")
                        .HasForeignKey("RezerwacjaId");
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
