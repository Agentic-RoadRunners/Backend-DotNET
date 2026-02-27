using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
public class UserJourney
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NetTopologySuite.Geometries.LineString RoutePath { get; set; } = null!;
    public JourneyStatus Status { get; set; } = JourneyStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;

    public ICollection<JourneyIncident> JourneyIncidents { get; set; } = new List<JourneyIncident>();
}