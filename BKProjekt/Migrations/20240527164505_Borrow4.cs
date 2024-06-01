using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKProjekt.Migrations
{
    /// <inheritdoc />
    public partial class Borrow4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "Borrows",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserName",
                table: "Borrows");
        }
    }
}
