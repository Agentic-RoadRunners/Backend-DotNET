using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

public class Comment
{
    public int Id { get; set; }
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Incident Incident { get; set; } = null!;
    public User User { get; set; } = null!;
}