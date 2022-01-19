using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class AddSessionLogged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "SessionLogged",
                table: "Cars",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SessionLogged",
                table: "Cars");
        }
    }
}
