using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookistry.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditablePropertyToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Subscriptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Subscriptions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Subscriptions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Subscriptions",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Reviews",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Reviews",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Reviews",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "ReadingProgresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "ReadingProgresses",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "ReadingProgresses",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "ReadingProgresses",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Payments",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Payments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Payments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Favorites",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Favorites",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Favorites",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Favorites",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Categories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Categories",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Categories",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Categories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "Books",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "Books",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "Books",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "Books",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UpdatedById",
                table: "Subscriptions",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UpdatedById",
                table: "Reviews",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ReadingProgresses_UpdatedById",
                table: "ReadingProgresses",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UpdatedById",
                table: "Payments",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_UpdatedById",
                table: "Favorites",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_UpdatedById",
                table: "Categories",
                column: "UpdatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Books_UpdatedById",
                table: "Books",
                column: "UpdatedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_AspNetUsers_UpdatedById",
                table: "Books",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_AspNetUsers_UpdatedById",
                table: "Categories",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_AspNetUsers_UpdatedById",
                table: "Favorites",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_AspNetUsers_UpdatedById",
                table: "Payments",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReadingProgresses_AspNetUsers_UpdatedById",
                table: "ReadingProgresses",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_UpdatedById",
                table: "Reviews",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_UpdatedById",
                table: "Subscriptions",
                column: "UpdatedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_AspNetUsers_UpdatedById",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Categories_AspNetUsers_UpdatedById",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_AspNetUsers_UpdatedById",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_AspNetUsers_UpdatedById",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReadingProgresses_AspNetUsers_UpdatedById",
                table: "ReadingProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_UpdatedById",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_UpdatedById",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_UpdatedById",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Reviews_UpdatedById",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_ReadingProgresses_UpdatedById",
                table: "ReadingProgresses");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UpdatedById",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_UpdatedById",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Categories_UpdatedById",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Books_UpdatedById",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "ReadingProgresses");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "ReadingProgresses");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "ReadingProgresses");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "ReadingProgresses");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "Books");
        }
    }
}
