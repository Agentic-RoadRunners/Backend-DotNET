using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;

namespace SafeRoad.Infrastructure.Seeds;

public static class RoleSeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "User" },
            new Role { Id = 3, Name = "Admin" },
            new Role { Id = 4, Name = "Municipality" }
        );
    }
}