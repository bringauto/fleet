using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class OrderCAll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CallTwiml",
                table: "Cars",
                defaultValue: "<say>New Order</say>",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarAdminPhone",
                table: "Cars",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CallTwiml",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "CarAdminPhone",
                table: "Cars");
        }
    }
}
