namespace SafeRoad.Core.DTOs.UserJourney;

public class UserJourneyResponse
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public double StartLatitude { get; set; }
    public double StartLongitude { get; set; }
    public double? EndLatitude { get; set; }
    public double? EndLongitude { get; set; }
    public string Status { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime? EndedAt { get; set; }
}