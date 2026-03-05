using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeRoad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemoveModeratorRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { 2, new Guid("a1a1a1a1-0000-0000-0000-000000000002") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("a1a1a1a1-0000-0000-0000-000000000002"));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: new Guid("a1a1a1a1-0000-0000-0000-000000000005"));

            migrationBuilder.UpdateData(
                table: "Verifications",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: new Guid("a1a1a1a1-0000-0000-0000-000000000001"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: new Guid("a1a1a1a1-0000-0000-0000-000000000002"));

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Moderator" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "CreatedAt", "Email", "FullName", "PasswordHash", "Status", "TrustScore" },
                values: new object[] { new Guid("a1a1a1a1-0000-0000-0000-000000000002"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "moderator@antalya.bel.tr", "Antalya Metro Moderator", "$2a$11$5J9R2E3V6vFmK1hL4nQ7aePwuXiLmnZ1HkD8vVA0QqGe3J7c2sMqi", "Active", 100 });

            migrationBuilder.UpdateData(
                table: "Verifications",
                keyColumn: "Id",
                keyValue: 2,
                column: "UserId",
                value: new Guid("a1a1a1a1-0000-0000-0000-000000000002"));

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { 2, new Guid("a1a1a1a1-0000-0000-0000-000000000002") });
        }
    }
}
