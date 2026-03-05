using System;
using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;

namespace SafeRoad.Infrastructure.Seeds;

public static class CommentSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>().HasData(
            new Comment
            {
                Id = 1,
                Content = "This pothole has been here for weeks, really dangerous!",
                IncidentId = IncidentSeedData.Incident1Id,
                UserId = UserSeedData.JaneSmithId,
                CreatedAt = new DateTime(2025, 1, 11, 9, 0, 0, DateTimeKind.Utc)
            },
            new Comment
            {
                Id = 2,
                Content = "Municipality team is on the way to fix this.",
                IncidentId = IncidentSeedData.Incident1Id,
                UserId = UserSeedData.MunOfficerId,
                CreatedAt = new DateTime(2025, 1, 12, 11, 0, 0, DateTimeKind.Utc)
            },
            new Comment
            {
                Id = 3,
                Content = "I almost had an accident because of this traffic light.",
                IncidentId = IncidentSeedData.Incident2Id,
                UserId = UserSeedData.JohnDoeId,
                CreatedAt = new DateTime(2025, 1, 13, 8, 30, 0, DateTimeKind.Utc)
            },
            new Comment
            {
                Id = 4,
                Content = "The flooding is really bad, avoid this road!",
                IncidentId = IncidentSeedData.Incident3Id,
                UserId = UserSeedData.JaneSmithId,
                CreatedAt = new DateTime(2025, 1, 15, 8, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}