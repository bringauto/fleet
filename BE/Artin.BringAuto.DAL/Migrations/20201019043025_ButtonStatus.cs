using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Artin.BringAuto.DAL.Migrations
{
    public partial class ButtonStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationHistory_Cars_CarId",
                table: "LocationHistory");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "LocationHistory",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Button",
                table: "Cars",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ButtonStates",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    CarId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ButtonStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ButtonStates_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ButtonStates_CarId",
                table: "ButtonStates",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationHistory_Cars_CarId",
                table: "LocationHistory",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationHistory_Cars_CarId",
                table: "LocationHistory");

            migrationBuilder.DropTable(
                name: "ButtonStates");

            migrationBuilder.DropColumn(
                name: "Button",
                table: "Cars");

            migrationBuilder.AlterColumn<int>(
                name: "CarId",
                table: "LocationHistory",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_LocationHistory_Cars_CarId",
                table: "LocationHistory",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
