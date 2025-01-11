using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace libraryManagment.EF.Migrations
{
    /// <inheritdoc />
    public partial class thirdMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "ISBN", "IsBorrowed", "Title" },
                values: new object[,]
                {
                    { 1, "Author 1", "1111", false, "Book 1" },
                    { 2, "Author 2", "2222", true, "Book 2" },
                    { 3, "Author 3", "3333", true, "Book 3" },
                    { 4, "Author 4", "4444", true, "Book 4" }
                });

            migrationBuilder.InsertData(
                table: "UserBooks",
                columns: new[] { "BookId", "UserId", "BorrowDate", "ReturnDate" },
                values: new object[] { 2, "24885ec7-d7a1-45be-85c2-e5bbb5e1774e", new DateTime(2025, 1, 11, 15, 14, 28, 398, DateTimeKind.Utc).AddTicks(7630), null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "UserBooks",
                keyColumns: new[] { "BookId", "UserId" },
                keyValues: new object[] { 2, "24885ec7-d7a1-45be-85c2-e5bbb5e1774e" });

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
