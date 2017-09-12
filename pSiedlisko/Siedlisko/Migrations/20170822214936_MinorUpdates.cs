using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Siedlisko.Migrations
{
    public partial class MinorUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prices_Rezerwacje_RezerwacjaId",
                table: "Prices");

            migrationBuilder.DropIndex(
                name: "IX_Prices_RezerwacjaId",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "RezerwacjaId",
                table: "Prices");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RezerwacjaId",
                table: "Prices",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prices_RezerwacjaId",
                table: "Prices",
                column: "RezerwacjaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prices_Rezerwacje_RezerwacjaId",
                table: "Prices",
                column: "RezerwacjaId",
                principalTable: "Rezerwacje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
