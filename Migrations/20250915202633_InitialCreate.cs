using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tollCalculator.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ActiveFeeDatas",
                columns: table => new
                {
                    ActiveFeeDataID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartCheckIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CurrentCheckIn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TotalFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveFeeDatas", x => x.ActiveFeeDataID);
                });

            migrationBuilder.CreateTable(
                name: "TollExempts",
                columns: table => new
                {
                    TollExemptID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsExempt = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TollExempts", x => x.TollExemptID);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    VehicleTypeID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(type: "int", nullable: false),
                    TollExemptID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.VehicleTypeID);
                    table.ForeignKey(
                        name: "FK_VehicleTypes_TollExempts_TollExemptID",
                        column: x => x.TollExemptID,
                        principalTable: "TollExempts",
                        principalColumn: "TollExemptID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    VehicleID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LicensePlate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    VehicleTypeID = table.Column<int>(type: "int", nullable: false),
                    ActiveFeeDataID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.VehicleID);
                    table.ForeignKey(
                        name: "FK_Vehicles_ActiveFeeDatas_ActiveFeeDataID",
                        column: x => x.ActiveFeeDataID,
                        principalTable: "ActiveFeeDatas",
                        principalColumn: "ActiveFeeDataID");
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeID",
                        column: x => x.VehicleTypeID,
                        principalTable: "VehicleTypes",
                        principalColumn: "VehicleTypeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeeDataReceipts",
                columns: table => new
                {
                    FeeDataReceiptID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FeeCheckInTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FeeAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VehicleID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeDataReceipts", x => x.FeeDataReceiptID);
                    table.ForeignKey(
                        name: "FK_FeeDataReceipts_Vehicles_VehicleID",
                        column: x => x.VehicleID,
                        principalTable: "Vehicles",
                        principalColumn: "VehicleID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeeDataReceipts_VehicleID",
                table: "FeeDataReceipts",
                column: "VehicleID");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ActiveFeeDataID",
                table: "Vehicles",
                column: "ActiveFeeDataID",
                unique: true,
                filter: "[ActiveFeeDataID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeID",
                table: "Vehicles",
                column: "VehicleTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleTypes_TollExemptID",
                table: "VehicleTypes",
                column: "TollExemptID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeDataReceipts");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "ActiveFeeDatas");

            migrationBuilder.DropTable(
                name: "VehicleTypes");

            migrationBuilder.DropTable(
                name: "TollExempts");
        }
    }
}
