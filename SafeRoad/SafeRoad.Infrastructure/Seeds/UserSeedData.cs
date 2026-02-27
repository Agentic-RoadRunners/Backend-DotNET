using System;
using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;

namespace SafeRoad.Infrastructure.Seeds;

public static class UserSeedData
{
    // Fixed GUIDs — must stay consistent across all seed files
    public static readonly Guid AdminId = new Guid("a1a1a1a1-0000-0000-0000-000000000001");
    public static readonly Guid ModeratorId = new Guid("a1a1a1a1-0000-0000-0000-000000000002");
    public static readonly Guid JohnDoeId = new Guid("a1a1a1a1-0000-0000-0000-000000000003");
    public static readonly Guid JaneSmithId = new Guid("a1a1a1a1-0000-0000-0000-000000000004");
    public static readonly Guid MunOfficerId = new Guid("a1a1a1a1-0000-0000-0000-000000000005");

    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = AdminId,
                FullName = "System Admin",
                Email = "admin@saferoad.com",
                PasswordHash = "$2a$11$5J9R2E3V6vFmK1hL4nQ7aeRSLZJk3C4BXB5oU6WpYgfUtwK0HbYpy", // Admin@123!
                Status = UserStatus.Active,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = ModeratorId,
                FullName = "Antalya Metro Moderator",
                Email = "moderator@antalya.bel.tr",
                PasswordHash = "$2a$11$5J9R2E3V6vFmK1hL4nQ7aePwuXiLmnZ1HkD8vVA0QqGe3J7c2sMqi", // Mod@123!
                Status = UserStatus.Active,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = JohnDoeId,
                FullName = "John Doe",
                Email = "john.doe@gmail.com",
                PasswordHash = "$2a$11$5J9R2E3V6vFmK1hL4nQ7aeT9kN2rM8b3GsDf5YiW0ExH6jR4lCvOu", // User@123!
                Status = UserStatus.Active,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = JaneSmithId,
                FullName = "Jane Smith",
                Email = "jane.smith@gmail.com",
                PasswordHash = "$2a$11$5J9R2E3V6vFmK1hL4nQ7aeT9kN2rM8b3GsDf5YiW0ExH6jR4lCvOu", // User@123!
                Status = UserStatus.Active,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new User
            {
                Id = MunOfficerId,
                FullName = "Kepez Municipality Officer",
                Email = "officer@kepez.bel.tr",
                PasswordHash = "$2a$11$5J9R2E3V6vFmK1hL4nQ7aeZVd4gCpO7RuFn9XbQ1ImK5tW2yEoMns", // Mun@123!
                Status = UserStatus.Active,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}