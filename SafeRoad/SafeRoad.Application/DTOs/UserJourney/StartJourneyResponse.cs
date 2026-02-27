namespace SafeRoad.Core.DTOs.UserJourney;

public class StartJourneyResponse
{
    public Guid JourneyId { get; set; }
    public double DistanceInKm { get; set; }
    public int EstimatedMinutes { get; set; }
    public int IncidentsOnRoute { get; set; }
    public List<RouteIncidentDto> Incidents { get; set; } = new();
    public string Message { get; set; } = null!;
}

public class RouteIncidentDto
{
    public Guid Id { get; set; }
    public string? Title { get; set; }
    public string CategoryName { get; set; } = null!;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double DistanceFromRouteMeters { get; set; }
    public string Status { get; set; } = null!;
}