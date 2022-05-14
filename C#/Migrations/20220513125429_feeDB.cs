using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace afryCodeTest.Migrations
{
    public partial class feeDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeeDay",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Day = table.Column<DateTime>(nullable: false),
                    FeeAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeDay", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "FeeHour",
                columns: table => new
                {
                    id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Time = table.Column<DateTime>(nullable: false),
                    FeeAmount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeHour", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicle",
                columns: table => new
                {
                    LicensePlate = table.Column<string>(nullable: false),
                    VehicleType = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicle", x => x.LicensePlate);
                });

            migrationBuilder.CreateTable(
                name: "Fees",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleLicensePlate = table.Column<string>(nullable: true),
                    FeeAmount = table.Column<int>(nullable: false),
                    FeeDayid = table.Column<ulong>(nullable: true),
                    FeeHourid = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Fees_FeeDay_FeeDayid",
                        column: x => x.FeeDayid,
                        principalTable: "FeeDay",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fees_FeeHour_FeeHourid",
                        column: x => x.FeeHourid,
                        principalTable: "FeeHour",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Fees_Vehicle_VehicleLicensePlate",
                        column: x => x.VehicleLicensePlate,
                        principalTable: "Vehicle",
                        principalColumn: "LicensePlate",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fees_FeeDayid",
                table: "Fees",
                column: "FeeDayid");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_FeeHourid",
                table: "Fees",
                column: "FeeHourid");

            migrationBuilder.CreateIndex(
                name: "IX_Fees_VehicleLicensePlate",
                table: "Fees",
                column: "VehicleLicensePlate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Fees");

            migrationBuilder.DropTable(
                name: "FeeDay");

            migrationBuilder.DropTable(
                name: "FeeHour");

            migrationBuilder.DropTable(
                name: "Vehicle");
        }
    }
}
