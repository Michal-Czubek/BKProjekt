using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKProjekt.Migrations
{
    /// <inheritdoc />
    public partial class Borrow89 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBorrowed",
                table: "Book");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Book",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Book");

            migrationBuilder.AddColumn<bool>(
                name: "IsBorrowed",
                table: "Book",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
