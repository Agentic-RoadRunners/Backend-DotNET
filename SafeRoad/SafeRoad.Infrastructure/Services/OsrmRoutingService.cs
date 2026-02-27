using System.Text.Json;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Interfaces.Services;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
namespace SafeRoad.Infrastructure.Services;

public class OsrmRoutingService : IRoutingService
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://router.project-osrm.org/route/v1/driving";

    public OsrmRoutingService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RouteResult?> GetRouteAsync(double startLat, double startLng, double endLat, double endLng)
    {
        try
        {
            var url = $"{BaseUrl}/{startLng},{startLat};{endLng},{endLat}?overview=full&geometries=geojson";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();
            var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.GetProperty("code").GetString() != "Ok")
                return null;

            var route = root.GetProperty("routes")[0];
            var distance = route.GetProperty("distance").GetDouble();
            var duration = route.GetProperty("duration").GetDouble();

            var coordinates = route.GetProperty("geometry").GetProperty("coordinates");
            var coords = new List<Coordinate>();

            foreach (var coord in coordinates.EnumerateArray())
            {
                var lng = coord[0].GetDouble();
                var lat = coord[1].GetDouble();
                coords.Add(new Coordinate(lng, lat));
            }

            if (coords.Count < 2)
                return null;

            var lineString = new LineString(coords.ToArray()) { SRID = 4326 };

            return new RouteResult
            {
                RoutePath = lineString,
                DistanceInMeters = distance,
                DurationInSeconds = duration
            };
        }
        catch
        {
            return null;
        }
    }
}