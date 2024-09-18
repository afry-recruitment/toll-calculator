using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AFRY.TollCalculator.API.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TollFeePeriods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TollFeePeriods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TollFreeDates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TollFreeDates", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TollFeePeriods",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "EndTime", "Fee", "ModifiedAt", "ModifiedBy", "StartTime" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 6, 29, 59, 0), 8m, null, null, new TimeSpan(0, 6, 0, 0, 0) },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 6, 59, 59, 0), 13m, null, null, new TimeSpan(0, 6, 30, 0, 0) },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 7, 59, 59, 0), 18m, null, null, new TimeSpan(0, 7, 0, 0, 0) },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 8, 29, 59, 0), 13m, null, null, new TimeSpan(0, 8, 0, 0, 0) },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 14, 59, 59, 0), 8m, null, null, new TimeSpan(0, 8, 30, 0, 0) },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 15, 29, 59, 0), 13m, null, null, new TimeSpan(0, 15, 0, 0, 0) },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 16, 59, 59, 0), 18m, null, null, new TimeSpan(0, 15, 30, 0, 0) },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 17, 59, 59, 0), 13m, null, null, new TimeSpan(0, 17, 0, 0, 0) },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", new TimeSpan(0, 18, 29, 59, 0), 8m, null, null, new TimeSpan(0, 18, 0, 0, 0) }
                });

            migrationBuilder.InsertData(
                table: "TollFreeDates",
                columns: new[] { "Id", "Date", "Description" },
                values: new object[,]
                {
                    { 1, new DateTime(2013, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year's Day" },
                    { 2, new DateTime(2013, 3, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "Good Friday" },
                    { 3, new DateTime(2013, 3, 29, 0, 0, 0, 0, DateTimeKind.Unspecified), "Easter Monday" },
                    { 4, new DateTime(2013, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holiday" },
                    { 5, new DateTime(2013, 4, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holiday" },
                    { 6, new DateTime(2013, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Labor Day" },
                    { 7, new DateTime(2013, 5, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holiday" },
                    { 8, new DateTime(2013, 5, 9, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holiday" },
                    { 9, new DateTime(2013, 6, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holiday" },
                    { 10, new DateTime(2013, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holiday" },
                    { 11, new DateTime(2013, 6, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holiday" },
                    { 12, new DateTime(2013, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Holiday" },
                    { 13, new DateTime(2013, 12, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christmas Eve" },
                    { 14, new DateTime(2013, 12, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Christmas Day" },
                    { 15, new DateTime(2013, 12, 26, 0, 0, 0, 0, DateTimeKind.Unspecified), "Boxing Day" },
                    { 16, new DateTime(2013, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "New Year's Eve" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TollFeePeriods");

            migrationBuilder.DropTable(
                name: "TollFreeDates");
        }
    }
}
