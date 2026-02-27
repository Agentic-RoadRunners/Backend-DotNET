using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

using SafeRoad.Core.Enums;
public class Incident
{
    public Guid Id { get; set; }
    public Guid ReporterUserId { get; set; }
    public int CategoryId { get; set; }
    public int? MunicipalityId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public NetTopologySuite.Geometries.Point Location { get; set; } = null!;
    public IncidentStatus Status { get; set; } = IncidentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User Reporter { get; set; } = null!;
    public IncidentCategory Category { get; set; } = null!;
    public Municipality? Municipality { get; set; }
    public ICollection<IncidentPhoto> Photos { get; set; } = new List<IncidentPhoto>();
    public ICollection<Verification> Verifications { get; set; } = new List<Verification>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public ICollection<JourneyIncident> JourneyIncidents { get; set; } = new List<JourneyIncident>();
}