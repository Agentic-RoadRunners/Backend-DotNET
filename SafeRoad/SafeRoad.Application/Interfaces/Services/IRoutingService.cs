using NetTopologySuite.Geometries;

namespace SafeRoad.Core.Interfaces.Services;

public class RouteResult
{
    public LineString RoutePath { get; set; } = null!;
    public double DistanceInMeters { get; set; }
    public double DurationInSeconds { get; set; }
}

public interface IRoutingService
{
    Task<RouteResult?> GetRouteAsync(double startLat, double startLng, double endLat, double endLng);
}