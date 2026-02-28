using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SafeRoad.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:postgis", ",,");

            migrationBuilder.CreateTable(
                name: "IncidentCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Municipalities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Boundary = table.Column<Polygon>(type: "geometry", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Municipalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    AvatarUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TrustScore = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DeviceTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    TokenString = table.Column<string>(type: "text", nullable: false),
                    DeviceType = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeviceTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeviceTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incidents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ReporterUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    MunicipalityId = table.Column<int>(type: "integer", nullable: true),
                    Title = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Location = table.Column<Point>(type: "geometry", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Incidents_IncidentCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "IncidentCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Incidents_Municipalities_MunicipalityId",
                        column: x => x.MunicipalityId,
                        principalTable: "Municipalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Incidents_Users_ReporterUserId",
                        column: x => x.ReporterUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserJourneys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoutePath = table.Column<LineString>(type: "geometry", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserJourneys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserJourneys_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WatchedAreas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Label = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Area = table.Column<Point>(type: "geometry", nullable: false),
                    RadiusInMeters = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WatchedAreas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WatchedAreas_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncidentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IncidentPhotos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncidentId = table.Column<Guid>(type: "uuid", nullable: false),
                    BlobUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentPhotos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IncidentPhotos_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Verifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IncidentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPositive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Verifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Verifications_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Verifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JourneyIncidents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    JourneyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IncidentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Feedback = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyIncidents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JourneyIncidents_Incidents_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JourneyIncidents_UserJourneys_JourneyId",
                        column: x => x.JourneyId,
                        principalTable: "UserJourneys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "IncidentCategories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Pothole" },
                    { 2, "Road Crack" },
                    { 3, "Broken Traffic Light" },
                    { 4, "Missing Road Sign" },
                    { 5, "Flooding" },
                    { 6, "Road Accident" },
                    { 7, "Obstacle on Road" },
                    { 8, "Broken Guardrail" },
                    { 9, "Damaged Sidewalk" },
                    { 10, "Street Light Out" }
                });

            migrationBuilder.InsertData(
                table: "Municipalities",
                columns: new[] { "Id", "Boundary", "Name" },
                values: new object[,]
                {
                    { 1, null, "Antalya Metropolitan Municipality" },
                    { 2, null, "Kepez Municipality" },
                    { 3, null, "Muratpaşa Municipality" },
                    { 4, null, "Konyaaltı Municipality" },
                    { 5, null, "Alanya Municipality" },
                    { 6, null, "Manavgat Municipality" },
                    { 7, null, "Serik Municipality" },
                    { 8, null, "Döşemealtı Municipality" },
                    { 9, null, "Aksu Municipality" },
                    { 10, null, "Gazipaşa Municipality" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "User" },
                    { 2, "Moderator" },
                    { 3, "Admin" },
                    { 4, "Municipality" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "AvatarUrl", "CreatedAt", "Email", "FullName", "PasswordHash", "Status", "TrustScore" },
                values: new object[,]
                {
                    { new Guid("a1a1a1a1-0000-0000-0000-000000000001"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin@saferoad.com", "System Admin", "$2a$11$5J9R2E3V6vFmK1hL4nQ7aeRSLZJk3C4BXB5oU6WpYgfUtwK0HbYpy", "Active", 100 },
                    { new Guid("a1a1a1a1-0000-0000-0000-000000000002"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "moderator@antalya.bel.tr", "Antalya Metro Moderator", "$2a$11$5J9R2E3V6vFmK1hL4nQ7aePwuXiLmnZ1HkD8vVA0QqGe3J7c2sMqi", "Active", 100 },
                    { new Guid("a1a1a1a1-0000-0000-0000-000000000003"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "john.doe@gmail.com", "John Doe", "$2a$11$5J9R2E3V6vFmK1hL4nQ7aeT9kN2rM8b3GsDf5YiW0ExH6jR4lCvOu", "Active", 100 },
                    { new Guid("a1a1a1a1-0000-0000-0000-000000000004"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "jane.smith@gmail.com", "Jane Smith", "$2a$11$5J9R2E3V6vFmK1hL4nQ7aeT9kN2rM8b3GsDf5YiW0ExH6jR4lCvOu", "Active", 100 },
                    { new Guid("a1a1a1a1-0000-0000-0000-000000000005"), null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "officer@kepez.bel.tr", "Kepez Municipality Officer", "$2a$11$5J9R2E3V6vFmK1hL4nQ7aeZVd4gCpO7RuFn9XbQ1ImK5tW2yEoMns", "Active", 100 }
                });

            migrationBuilder.InsertData(
                table: "Incidents",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "Location", "MunicipalityId", "ReporterUserId", "Status", "Title" },
                values: new object[,]
                {
                    { new Guid("b2b2b2b2-0000-0000-0000-000000000001"), 1, new DateTime(2025, 1, 10, 8, 0, 0, 0, DateTimeKind.Utc), "Deep pothole causing vehicle damage near the city center.", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (30.7133 36.8969)"), 1, new Guid("a1a1a1a1-0000-0000-0000-000000000003"), "Pending", "Large Pothole on Atatürk Boulevard" },
                    { new Guid("b2b2b2b2-0000-0000-0000-000000000002"), 3, new DateTime(2025, 1, 12, 10, 30, 0, 0, DateTimeKind.Utc), "Traffic light has been non-functional for 3 days.", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (30.7 36.88)"), 3, new Guid("a1a1a1a1-0000-0000-0000-000000000004"), "Verified", "Broken Traffic Light at Işıklar Street" },
                    { new Guid("b2b2b2b2-0000-0000-0000-000000000003"), 5, new DateTime(2025, 1, 15, 7, 0, 0, 0, DateTimeKind.Utc), "Street is flooded after heavy rain, making it impassable.", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (30.65 36.87)"), 4, new Guid("a1a1a1a1-0000-0000-0000-000000000003"), "Pending", "Flooding on Konyaaltı Beach Road" },
                    { new Guid("b2b2b2b2-0000-0000-0000-000000000004"), 4, new DateTime(2025, 1, 5, 14, 0, 0, 0, DateTimeKind.Utc), "Speed limit sign is missing after an accident.", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (31.99 36.544)"), 5, new Guid("a1a1a1a1-0000-0000-0000-000000000004"), "Resolved", "Missing Road Sign on Alanya Highway" },
                    { new Guid("b2b2b2b2-0000-0000-0000-000000000005"), 2, new DateTime(2025, 1, 18, 9, 0, 0, 0, DateTimeKind.Utc), "Long crack across the road causing safety concerns.", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (30.73 37.01)"), 2, new Guid("a1a1a1a1-0000-0000-0000-000000000003"), "Pending", "Road Crack on Kepez Industrial Zone" }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { 3, new Guid("a1a1a1a1-0000-0000-0000-000000000001") },
                    { 2, new Guid("a1a1a1a1-0000-0000-0000-000000000002") },
                    { 1, new Guid("a1a1a1a1-0000-0000-0000-000000000003") },
                    { 1, new Guid("a1a1a1a1-0000-0000-0000-000000000004") },
                    { 4, new Guid("a1a1a1a1-0000-0000-0000-000000000005") }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "Id", "Content", "CreatedAt", "IncidentId", "UserId" },
                values: new object[,]
                {
                    { 1, "This pothole has been here for weeks, really dangerous!", new DateTime(2025, 1, 11, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000001"), new Guid("a1a1a1a1-0000-0000-0000-000000000004") },
                    { 2, "Municipality team is on the way to fix this.", new DateTime(2025, 1, 12, 11, 0, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000001"), new Guid("a1a1a1a1-0000-0000-0000-000000000002") },
                    { 3, "I almost had an accident because of this traffic light.", new DateTime(2025, 1, 13, 8, 30, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000002"), new Guid("a1a1a1a1-0000-0000-0000-000000000003") },
                    { 4, "The flooding is really bad, avoid this road!", new DateTime(2025, 1, 15, 8, 0, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000003"), new Guid("a1a1a1a1-0000-0000-0000-000000000004") }
                });

            migrationBuilder.InsertData(
                table: "Verifications",
                columns: new[] { "Id", "CreatedAt", "IncidentId", "IsPositive", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 11, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000001"), true, new Guid("a1a1a1a1-0000-0000-0000-000000000004") },
                    { 2, new DateTime(2025, 1, 11, 12, 0, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000001"), true, new Guid("a1a1a1a1-0000-0000-0000-000000000002") },
                    { 3, new DateTime(2025, 1, 13, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000002"), true, new Guid("a1a1a1a1-0000-0000-0000-000000000003") },
                    { 4, new DateTime(2025, 1, 15, 9, 0, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000003"), true, new Guid("a1a1a1a1-0000-0000-0000-000000000004") },
                    { 5, new DateTime(2025, 1, 6, 10, 0, 0, 0, DateTimeKind.Utc), new Guid("b2b2b2b2-0000-0000-0000-000000000004"), false, new Guid("a1a1a1a1-0000-0000-0000-000000000003") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_IncidentId",
                table: "Comments",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DeviceTokens_UserId",
                table: "DeviceTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentPhotos_IncidentId",
                table: "IncidentPhotos",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_CategoryId",
                table: "Incidents",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_MunicipalityId",
                table: "Incidents",
                column: "MunicipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_Incidents_ReporterUserId",
                table: "Incidents",
                column: "ReporterUserId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyIncidents_IncidentId",
                table: "JourneyIncidents",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyIncidents_JourneyId",
                table: "JourneyIncidents",
                column: "JourneyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserJourneys_UserId",
                table: "UserJourneys",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_IncidentId",
                table: "Verifications",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_Verifications_UserId",
                table: "Verifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WatchedAreas_UserId",
                table: "WatchedAreas",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "DeviceTokens");

            migrationBuilder.DropTable(
                name: "IncidentPhotos");

            migrationBuilder.DropTable(
                name: "JourneyIncidents");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Verifications");

            migrationBuilder.DropTable(
                name: "WatchedAreas");

            migrationBuilder.DropTable(
                name: "UserJourneys");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Incidents");

            migrationBuilder.DropTable(
                name: "IncidentCategories");

            migrationBuilder.DropTable(
                name: "Municipalities");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
