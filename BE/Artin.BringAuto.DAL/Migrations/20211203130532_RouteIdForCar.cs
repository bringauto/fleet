using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class RouteIdForCar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RouteId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cars_RouteId",
                table: "Cars",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Routes_RouteId",
                table: "Cars",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Routes_RouteId",
                table: "Cars");

            migrationBuilder.DropIndex(
                name: "IX_Cars_RouteId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "RouteId",
                table: "Cars");
        }
    }
}
