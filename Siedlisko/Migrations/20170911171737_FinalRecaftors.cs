using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Siedlisko.Migrations
{
    public partial class FinalRecaftors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezerwacje_Pokoje_PokojId",
                table: "Rezerwacje");

            migrationBuilder.RenameColumn(
                name: "PokojId",
                table: "Rezerwacje",
                newName: "RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Rezerwacje_PokojId",
                table: "Rezerwacje",
                newName: "IX_Rezerwacje_RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezerwacje_Pokoje_RoomId",
                table: "Rezerwacje",
                column: "RoomId",
                principalTable: "Pokoje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rezerwacje_Pokoje_RoomId",
                table: "Rezerwacje");

            migrationBuilder.RenameColumn(
                name: "RoomId",
                table: "Rezerwacje",
                newName: "PokojId");

            migrationBuilder.RenameIndex(
                name: "IX_Rezerwacje_RoomId",
                table: "Rezerwacje",
                newName: "IX_Rezerwacje_PokojId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rezerwacje_Pokoje_PokojId",
                table: "Rezerwacje",
                column: "PokojId",
                principalTable: "Pokoje",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
