using System;
using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;

namespace SafeRoad.Infrastructure.Seeds;

public static class VerificationSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Verification>().HasData(
            new Verification { Id = 1, IncidentId = IncidentSeedData.Incident1Id, UserId = UserSeedData.JaneSmithId, IsPositive = true, CreatedAt = new DateTime(2025, 1, 11, 10, 0, 0, DateTimeKind.Utc) },
            new Verification { Id = 2, IncidentId = IncidentSeedData.Incident1Id, UserId = UserSeedData.ModeratorId, IsPositive = true, CreatedAt = new DateTime(2025, 1, 11, 12, 0, 0, DateTimeKind.Utc) },
            new Verification { Id = 3, IncidentId = IncidentSeedData.Incident2Id, UserId = UserSeedData.JohnDoeId, IsPositive = true, CreatedAt = new DateTime(2025, 1, 13, 9, 0, 0, DateTimeKind.Utc) },
            new Verification { Id = 4, IncidentId = IncidentSeedData.Incident3Id, UserId = UserSeedData.JaneSmithId, IsPositive = true, CreatedAt = new DateTime(2025, 1, 15, 9, 0, 0, DateTimeKind.Utc) },
            new Verification { Id = 5, IncidentId = IncidentSeedData.Incident4Id, UserId = UserSeedData.JohnDoeId, IsPositive = false, CreatedAt = new DateTime(2025, 1, 6, 10, 0, 0, DateTimeKind.Utc) }
        );
    }
}