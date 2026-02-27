using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

public class IncidentCategory
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;

    // Navigation property
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}