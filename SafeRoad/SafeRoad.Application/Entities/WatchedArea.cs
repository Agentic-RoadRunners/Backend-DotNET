using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

public class WatchedArea
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string? Label { get; set; }
    public NetTopologySuite.Geometries.Point Area { get; set; } = null!;
    public int? RadiusInMeters { get; set; }

    public User User { get; set; } = null!;
}