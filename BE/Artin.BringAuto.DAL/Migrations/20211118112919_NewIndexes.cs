using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class NewIndexes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_LocationHistory_Time",
                table: "LocationHistory",
                column: "Time");

            migrationBuilder.CreateIndex(
                name: "IX_LocationHistory_Latitude_Longitude_Time",
                table: "LocationHistory",
                columns: new[] { "Latitude", "Longitude", "Time" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LocationHistory_Time",
                table: "LocationHistory");

            migrationBuilder.DropIndex(
                name: "IX_LocationHistory_Latitude_Longitude_Time",
                table: "LocationHistory");
        }
    }
}
