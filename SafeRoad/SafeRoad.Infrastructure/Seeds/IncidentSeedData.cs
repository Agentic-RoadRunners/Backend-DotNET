using System;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;

namespace SafeRoad.Infrastructure.Seeds;

public static class IncidentSeedData
{
    public static readonly Guid Incident1Id = new Guid("b2b2b2b2-0000-0000-0000-000000000001");
    public static readonly Guid Incident2Id = new Guid("b2b2b2b2-0000-0000-0000-000000000002");
    public static readonly Guid Incident3Id = new Guid("b2b2b2b2-0000-0000-0000-000000000003");
    public static readonly Guid Incident4Id = new Guid("b2b2b2b2-0000-0000-0000-000000000004");
    public static readonly Guid Incident5Id = new Guid("b2b2b2b2-0000-0000-0000-000000000005");

    public static void Seed(ModelBuilder modelBuilder)
    {
        var geometryFactory = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(srid: 4326);

        modelBuilder.Entity<Incident>().HasData(
            new Incident
            {
                Id = Incident1Id,
                Title = "Large Pothole on Atatürk Boulevard",
                Description = "Deep pothole causing vehicle damage near the city center.",
                Location = geometryFactory.CreatePoint(new Coordinate(30.7133, 36.8969)),
                Status = IncidentStatus.Pending,
                CategoryId = 1,
                ReporterUserId = UserSeedData.JohnDoeId,
                MunicipalityId = 1,
                CreatedAt = new DateTime(2025, 1, 10, 8, 0, 0, DateTimeKind.Utc)
            },
            new Incident
            {
                Id = Incident2Id,
                Title = "Broken Traffic Light at Işıklar Street",
                Description = "Traffic light has been non-functional for 3 days.",
                Location = geometryFactory.CreatePoint(new Coordinate(30.7000, 36.8800)),
                Status = IncidentStatus.Verified,
                CategoryId = 3,
                ReporterUserId = UserSeedData.JaneSmithId,
                MunicipalityId = 3,
                CreatedAt = new DateTime(2025, 1, 12, 10, 30, 0, DateTimeKind.Utc)
            },
            new Incident
            {
                Id = Incident3Id,
                Title = "Flooding on Konyaaltı Beach Road",
                Description = "Street is flooded after heavy rain, making it impassable.",
                Location = geometryFactory.CreatePoint(new Coordinate(30.6500, 36.8700)),
                Status = IncidentStatus.Pending,
                CategoryId = 5,
                ReporterUserId = UserSeedData.JohnDoeId,
                MunicipalityId = 4,
                CreatedAt = new DateTime(2025, 1, 15, 7, 0, 0, DateTimeKind.Utc)
            },
            new Incident
            {
                Id = Incident4Id,
                Title = "Missing Road Sign on Alanya Highway",
                Description = "Speed limit sign is missing after an accident.",
                Location = geometryFactory.CreatePoint(new Coordinate(31.9900, 36.5440)),
                Status = IncidentStatus.Resolved,
                CategoryId = 4,
                ReporterUserId = UserSeedData.JaneSmithId,
                MunicipalityId = 5,
                CreatedAt = new DateTime(2025, 1, 5, 14, 0, 0, DateTimeKind.Utc)
            },
            new Incident
            {
                Id = Incident5Id,
                Title = "Road Crack on Kepez Industrial Zone",
                Description = "Long crack across the road causing safety concerns.",
                Location = geometryFactory.CreatePoint(new Coordinate(30.7300, 37.0100)),
                Status = IncidentStatus.Pending,
                CategoryId = 2,
                ReporterUserId = UserSeedData.JohnDoeId,
                MunicipalityId = 2,
                CreatedAt = new DateTime(2025, 1, 18, 9, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}