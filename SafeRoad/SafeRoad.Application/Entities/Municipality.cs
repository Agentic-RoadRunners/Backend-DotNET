using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

public class Municipality
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public NetTopologySuite.Geometries.Polygon? Boundary { get; set; }

    // Navigation properties
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
}