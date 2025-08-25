using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bookistry.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertyToPaymentUser2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FailureReason",
                table: "Payments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FailureReason",
                table: "Payments",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);
        }
    }
}
