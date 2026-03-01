using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;

namespace SafeRoad.Infrastructure.Seeds;

public static class UserRoleSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole { UserId = UserSeedData.AdminId, RoleId = 3 },  // Admin
            new UserRole { UserId = UserSeedData.ModeratorId, RoleId = 2 },  // Moderator
            new UserRole { UserId = UserSeedData.JohnDoeId, RoleId = 1 },  // User
            new UserRole { UserId = UserSeedData.JaneSmithId, RoleId = 1 },  // User
            new UserRole { UserId = UserSeedData.MunOfficerId, RoleId = 4 },  // Municipality
            new UserRole { UserId = UserSeedData.HatayMunId, RoleId = 4 }     // Municipality
        );
    }
}