using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKProjekt.Migrations
{
    /// <inheritdoc />
    public partial class Borrow5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Borrows",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Borrows");
        }
    }
}
