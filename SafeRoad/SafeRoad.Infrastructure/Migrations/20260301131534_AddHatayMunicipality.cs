using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeRoad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHatayMunicipality : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Municipalities",
                columns: new[] { "Id", "Boundary", "Name" },
                values: new object[] { 11, null, "Hatay Belediyesi" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "CreatedAt", "Email", "FullName", "PasswordHash", "Status", "TrustScore" },
                values: new object[] { new Guid("a1a1a1a1-0000-0000-0000-000000000006"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hatay@belediye.gov.tr", "Hatay Belediyesi", "$2a$11$DsWH6jqqsre2kV4MTomJZe6Q4/TkI0PJbaCitjjxC3QXKZzxJqaKK", "Active", 100 });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 4, new Guid("a1a1a1a1-0000-0000-0000-000000000006") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Municipalities",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 4, new Guid("a1a1a1a1-0000-0000-0000-000000000006") });

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-0000-0000-0000-000000000006"));
        }
    }
}
