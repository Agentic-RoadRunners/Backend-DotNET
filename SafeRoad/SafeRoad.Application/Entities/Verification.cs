using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

public class Verification
{
    public int Id { get; set; }
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
    public bool IsPositive { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Incident Incident { get; set; } = null!;
    public User User { get; set; } = null!;
}