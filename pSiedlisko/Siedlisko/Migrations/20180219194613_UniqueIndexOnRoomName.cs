using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Siedlisko.Migrations
{
    public partial class UniqueIndexOnRoomName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pokoje",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Pokoje_Name",
                table: "Pokoje",
                column: "Name",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Pokoje_Name",
                table: "Pokoje");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Pokoje",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
