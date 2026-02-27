
namespace SafeRoad.Core.DTOs.WatchedArea;

public class CreateWatchedAreaRequest
{
    public string? Label { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int RadiusInMeters { get; set; }
}