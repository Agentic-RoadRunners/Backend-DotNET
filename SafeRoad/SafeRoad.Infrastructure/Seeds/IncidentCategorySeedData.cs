using Microsoft.EntityFrameworkCore;
using SafeRoad.Core.Entities;

namespace SafeRoad.Infrastructure.Seeds;

public static class IncidentCategorySeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IncidentCategory>().HasData(
            new IncidentCategory { Id = 1, Name = "Pothole" },
            new IncidentCategory { Id = 2, Name = "Road Crack" },
            new IncidentCategory { Id = 3, Name = "Broken Traffic Light" },
            new IncidentCategory { Id = 4, Name = "Missing Road Sign" },
            new IncidentCategory { Id = 5, Name = "Flooding" },
            new IncidentCategory { Id = 6, Name = "Road Accident" },
            new IncidentCategory { Id = 7, Name = "Obstacle on Road" },
            new IncidentCategory { Id = 8, Name = "Broken Guardrail" },
            new IncidentCategory { Id = 9, Name = "Damaged Sidewalk" },
            new IncidentCategory { Id = 10, Name = "Street Light Out" }
        );
    }
}