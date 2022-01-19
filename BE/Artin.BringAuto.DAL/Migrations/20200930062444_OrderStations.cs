using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class OrderStations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stations_FromId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stations_ToId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FromId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ToId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "FromId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ToId",
                table: "Orders");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FromStationId",
                table: "Orders",
                column: "FromStationId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ToStationId",
                table: "Orders",
                column: "ToStationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stations_FromStationId",
                table: "Orders",
                column: "FromStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stations_ToStationId",
                table: "Orders",
                column: "ToStationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stations_FromStationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stations_ToStationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_FromStationId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ToStationId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "FromId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ToId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_FromId",
                table: "Orders",
                column: "FromId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ToId",
                table: "Orders",
                column: "ToId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stations_FromId",
                table: "Orders",
                column: "FromId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stations_ToId",
                table: "Orders",
                column: "ToId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
