using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class FromAndToStationPhone : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FromStationPhone",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ToStationPhone",
                table: "Orders",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromStationPhone",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ToStationPhone",
                table: "Orders");
        }
    }
}
