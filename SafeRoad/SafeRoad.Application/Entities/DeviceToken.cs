using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;

public class DeviceToken
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string TokenString { get; set; } = null!;
    public string? DeviceType { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation property
    public User User { get; set; } = null!;
}