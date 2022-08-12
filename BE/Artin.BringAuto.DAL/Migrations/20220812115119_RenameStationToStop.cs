using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class RenameStationToStop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stations_FromStationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stations_ToStationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteStops_Stations_StationId",
                table: "RouteStops");

            migrationBuilder.RenameTable(name: "Stations", newName: "Stops");
            
            migrationBuilder.CreateIndex(
                name: "IX_Stops_TenantId",
                table: "Stops",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stops_FromStationId",
                table: "Orders",
                column: "FromStationId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Stops_ToStationId",
                table: "Orders",
                column: "ToStationId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStops_Stops_StationId",
                table: "RouteStops",
                column: "StationId",
                principalTable: "Stops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stops_FromStationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Stops_ToStationId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteStops_Stops_StationId",
                table: "RouteStops");

            migrationBuilder.DropTable(
                name: "Stops");

            migrationBuilder.CreateTable(
                name: "Stations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stations_Tenants_TenantId",
                        column: x => x.TenantId,
                        principalTable: "Tenants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_TenantId",
                table: "Stations",
                column: "TenantId");

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
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStops_Stations_StationId",
                table: "RouteStops",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
