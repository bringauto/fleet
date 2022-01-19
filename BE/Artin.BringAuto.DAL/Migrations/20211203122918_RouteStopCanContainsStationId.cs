using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class RouteStopCanContainsStationId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "RouteStops",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_StationId",
                table: "RouteStops",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStops_Stations_StationId",
                table: "RouteStops",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteStops_Stations_StationId",
                table: "RouteStops");

            migrationBuilder.DropIndex(
                name: "IX_RouteStops_StationId",
                table: "RouteStops");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "RouteStops");
        }
    }
}
