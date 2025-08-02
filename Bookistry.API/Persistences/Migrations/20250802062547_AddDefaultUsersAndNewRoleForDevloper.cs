using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Bookistry.API.Persistences.Migrations
{
    /// <inheritdoc />
    public partial class AddDefaultUsersAndNewRoleForDevloper : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[] { "8757DDE1-DA74-4A92-9EEB-46C4A35AC090", "F167EA47-FC22-4A47-81F9-1E21C11DB217", false, false, "Developer", "DEVELOPER" });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "IsVIP", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "ProfileImageUrl", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "4E14506C-D3C0-4AE3-8616-5EB95A764358", 0, "CE9E600E-ECD5-4400-92E6-986F63EEC953", "dev@mohamed.com", true, "Mohamed", false, true, "El Sayed", false, null, "DEV@MOHAMED.COM", "DEV@MOHAMED.COM", "AQAAAAIAAYagAAAAEGBGVUvSYxVEnNBXm5kHe/mW8CYkFMU3yKabkMNZUl35T1pl1Qh165+GvKM0kus7qQ==", "+201002308834", true, "", "2FCB053BC1F041F2B07D3E7608D8020E", false, "dev@mohamed.com" },
                    { "DADA3B40-21CE-482A-8295-1C466E4B2B83", 0, "9422E5CF-06D6-422D-B3A6-C7E1E34B3A1C", "admin@mahmoud.com", true, "Mahmoud", false, true, "Yasser", false, null, "ADMIN@MAHMOUD.COM", "ADMIN@MAHMOUD.COM", "AQAAAAIAAYagAAAAEKj70KPmPc7BxyRhD9MuptCGolRkbmTp27lM/5HLVQxdU/qZw0HwYDAGR9JyB4c19Q==", null, false, "", "78A73231C42F47D4B13D2CF4A3672B51", false, "admin@mahmoud.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "8757DDE1-DA74-4A92-9EEB-46C4A35AC090", "4E14506C-D3C0-4AE3-8616-5EB95A764358" },
                    { "868826A7-5589-4BF0-82DA-5E04408ADC8F", "DADA3B40-21CE-482A-8295-1C466E4B2B83" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "8757DDE1-DA74-4A92-9EEB-46C4A35AC090", "4E14506C-D3C0-4AE3-8616-5EB95A764358" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "868826A7-5589-4BF0-82DA-5E04408ADC8F", "DADA3B40-21CE-482A-8295-1C466E4B2B83" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8757DDE1-DA74-4A92-9EEB-46C4A35AC090");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "4E14506C-D3C0-4AE3-8616-5EB95A764358");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "DADA3B40-21CE-482A-8295-1C466E4B2B83");
        }
    }
}
