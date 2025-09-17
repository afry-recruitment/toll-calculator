using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace tollCalculator.Migrations
{
    /// <inheritdoc />
    public partial class minorChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "ActiveFeeDatas");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "ActiveFeeDatas",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
