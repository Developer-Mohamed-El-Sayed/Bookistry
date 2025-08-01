using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookistry.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddRolesToAspNetRolesTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4D447E8A-B35A-4DAE-BCE3-4552BF828693", "E9FD0D85-6770-4A99-B3A2-69158B9EF3D7", true, false, "Reader", "READER" },
                    { "868826A7-5589-4BF0-82DA-5E04408ADC8F", "13071EF4-9B9D-4594-804F-1E8650DA4417", false, false, "Author", "AUTHOR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4D447E8A-B35A-4DAE-BCE3-4552BF828693");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "868826A7-5589-4BF0-82DA-5E04408ADC8F");
        }
    }
}
