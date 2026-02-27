using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;

namespace SafeRoad.Infrastructure.Seeds;

public static class MunicipalitySeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Municipality>().HasData(
            new Municipality { Id = 1, Name = "Antalya Metropolitan Municipality" },
            new Municipality { Id = 2, Name = "Kepez Municipality" },
            new Municipality { Id = 3, Name = "Muratpaşa Municipality" },
            new Municipality { Id = 4, Name = "Konyaaltı Municipality" },
            new Municipality { Id = 5, Name = "Alanya Municipality" },
            new Municipality { Id = 6, Name = "Manavgat Municipality" },
            new Municipality { Id = 7, Name = "Serik Municipality" },
            new Municipality { Id = 8, Name = "Döşemealtı Municipality" },
            new Municipality { Id = 9, Name = "Aksu Municipality" },
            new Municipality { Id = 10, Name = "Gazipaşa Municipality" }
        );
    }
}