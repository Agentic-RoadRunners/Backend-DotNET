using System;
using System.Collections.Generic;
namespace SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
public class User
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string PasswordHash { get; set; } = null!;
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public int TrustScore { get; set; } = 100;
    public UserStatus Status { get; set; } = UserStatus.Active;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public ICollection<Incident> ReportedIncidents { get; set; } = new List<Incident>();
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    public ICollection<Verification> Verifications { get; set; } = new List<Verification>();
    public ICollection<DeviceToken> DeviceTokens { get; set; } = new List<DeviceToken>();
    public ICollection<UserJourney> UserJourneys { get; set; } = new List<UserJourney>();
    public ICollection<WatchedArea> WatchedAreas { get; set; } = new List<WatchedArea>();
}