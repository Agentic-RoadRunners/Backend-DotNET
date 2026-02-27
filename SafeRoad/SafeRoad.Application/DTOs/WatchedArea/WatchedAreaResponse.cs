
namespace SafeRoad.Core.DTOs.WatchedArea;

public class WatchedAreaResponse
{
    public int Id { get; set; }
    public string? Label { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int? RadiusInMeters { get; set; }
}