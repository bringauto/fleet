using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class TenancyData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Stations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "RouteStops",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Routes",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Maps",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Tenants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tenants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stations_TenantId",
                table: "Stations",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_RouteStops_TenantId",
                table: "RouteStops",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Routes_TenantId",
                table: "Routes",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TenantId",
                table: "Orders",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Maps_TenantId",
                table: "Maps",
                column: "TenantId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_TenantId",
                table: "Cars",
                column: "TenantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Tenants_TenantId",
                table: "Cars",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Maps_Tenants_TenantId",
                table: "Maps",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Tenants_TenantId",
                table: "Orders",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Routes_Tenants_TenantId",
                table: "Routes",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RouteStops_Tenants_TenantId",
                table: "RouteStops",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Tenants_TenantId",
                table: "Stations",
                column: "TenantId",
                principalTable: "Tenants",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Tenants_TenantId",
                table: "Cars");

            migrationBuilder.DropForeignKey(
                name: "FK_Maps_Tenants_TenantId",
                table: "Maps");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Tenants_TenantId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Routes_Tenants_TenantId",
                table: "Routes");

            migrationBuilder.DropForeignKey(
                name: "FK_RouteStops_Tenants_TenantId",
                table: "RouteStops");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Tenants_TenantId",
                table: "Stations");

            migrationBuilder.DropTable(
                name: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Stations_TenantId",
                table: "Stations");

            migrationBuilder.DropIndex(
                name: "IX_RouteStops_TenantId",
                table: "RouteStops");

            migrationBuilder.DropIndex(
                name: "IX_Routes_TenantId",
                table: "Routes");

            migrationBuilder.DropIndex(
                name: "IX_Orders_TenantId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Maps_TenantId",
                table: "Maps");

            migrationBuilder.DropIndex(
                name: "IX_Cars_TenantId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "RouteStops");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Routes");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Maps");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Cars");
        }
    }
}
