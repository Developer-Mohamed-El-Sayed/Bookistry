using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookistry.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddIndexAtReviewTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId_ReviewerId",
                table: "Reviews",
                columns: new[] { "BookId", "ReviewerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Reviews_BookId_ReviewerId",
                table: "Reviews");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                column: "BookId");
        }
    }
}
