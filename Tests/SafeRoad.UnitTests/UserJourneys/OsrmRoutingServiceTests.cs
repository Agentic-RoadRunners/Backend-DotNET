using FluentAssertions;
using SafeRoad.Infrastructure.Services;

namespace SafeRoad.UnitTests.UserJourneys;

public class OsrmRoutingServiceTests
{
    private readonly OsrmRoutingService _service;

    public OsrmRoutingServiceTests()
    {
        _service = new OsrmRoutingService(new HttpClient());
    }

    [Fact]
    public async Task GetRouteAsync_ValidCoordinates_ReturnsRoute()
    {
        // Sultanahmet → Beşiktaş
        var result = await _service.GetRouteAsync(41.0082, 28.9784, 41.0432, 29.0090);

        result.Should().NotBeNull();
        result!.DistanceInMeters.Should().BeGreaterThan(0);
        result.DurationInSeconds.Should().BeGreaterThan(0);
        result.RoutePath.NumPoints.Should().BeGreaterThan(2);
    }

    [Fact]
    public async Task GetRouteAsync_SameStartAndEnd_ReturnsRoute()
    {
        var result = await _service.GetRouteAsync(41.0082, 28.9784, 41.0082, 28.9784);

        result.Should().NotBeNull();
        result!.DistanceInMeters.Should().Be(0);
    }

    [Fact]
    public async Task GetRouteAsync_LongDistance_ReturnsRouteWithDetails()
    {
        // İstanbul → Ankara
        var result = await _service.GetRouteAsync(41.0082, 28.9784, 39.9334, 32.8597);

        result.Should().NotBeNull();
        result!.DistanceInMeters.Should().BeGreaterThan(400000); // ~450km
        result.RoutePath.NumPoints.Should().BeGreaterThan(100);
    }
}