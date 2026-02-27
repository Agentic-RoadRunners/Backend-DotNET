namespace SafeRoad.Core.DTOs.UserJourney;

public class EndJourneyResponse
{
    public Guid JourneyId { get; set; }
    public double DistanceInKm { get; set; }
    public int DurationMinutes { get; set; }
    public string Message { get; set; } = null!;
    public bool AskForIncidentReport { get; set; }
    public string? IncidentPrompt { get; set; }
}