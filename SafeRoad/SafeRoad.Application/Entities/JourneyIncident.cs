using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

public class JourneyIncident
{
    public int Id { get; set; }
    public Guid JourneyId { get; set; }
    public Guid IncidentId { get; set; }
    public bool? Feedback { get; set; }

    // Navigation properties
    public UserJourney Journey { get; set; } = null!;
    public Incident Incident { get; set; } = null!;
}