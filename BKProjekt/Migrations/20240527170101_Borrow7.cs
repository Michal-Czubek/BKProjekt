using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BKProjekt.Migrations
{
    /// <inheritdoc />
    public partial class Borrow7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorrowId",
                table: "Book",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_BorrowId",
                table: "Book",
                column: "BorrowId");

            migrationBuilder.AddForeignKey(
                name: "FK_Book_Borrows_BorrowId",
                table: "Book",
                column: "BorrowId",
                principalTable: "Borrows",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Book_Borrows_BorrowId",
                table: "Book");

            migrationBuilder.DropIndex(
                name: "IX_Book_BorrowId",
                table: "Book");

            migrationBuilder.DropColumn(
                name: "BorrowId",
                table: "Book");
        }
    }
}
