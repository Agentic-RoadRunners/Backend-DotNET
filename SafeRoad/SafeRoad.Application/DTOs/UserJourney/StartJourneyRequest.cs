namespace SafeRoad.Core.DTOs.UserJourney;

public class StartJourneyRequest
{
    public double StartLatitude { get; set; }
    public double StartLongitude { get; set; }
    public double EndLatitude { get; set; }
    public double EndLongitude { get; set; }
}