
namespace SafeRoad.Core.DTOs.Incident;

public class NearbyIncidentsRequest
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int RadiusMeters { get; set; } = 5000;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}