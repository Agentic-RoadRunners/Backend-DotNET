using Moq;
using FluentAssertions;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Interfaces.Services;
using SafeRoad.Core.Features.UserJourneys.Commands.StartJourney;

namespace SafeRoad.UnitTests.UserJourneys;

public class StartJourneyCommandHandlerTests
{
    private readonly Mock<IUserJourneyRepository> _journeyRepoMock = new();
    private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
    private readonly Mock<IRoutingService> _routingServiceMock = new();
    private readonly StartJourneyCommandHandler _handler;

    public StartJourneyCommandHandlerTests()
    {
        _handler = new StartJourneyCommandHandler(
            _journeyRepoMock.Object,
            _incidentRepoMock.Object,
            _routingServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidRoute_ReturnsSuccessWithIncidents()
    {
        var userId = Guid.NewGuid();
        var routePath = new LineString(new[]
        {
            new Coordinate(28.9784, 41.0082),
            new Coordinate(29.0090, 41.0432)
        })
        { SRID = 4326 };

        _journeyRepoMock.Setup(x => x.GetActiveByUserIdAsync(userId)).ReturnsAsync((UserJourney?)null);
        _routingServiceMock.Setup(x => x.GetRouteAsync(41.0082, 28.9784, 41.0432, 29.0090))
            .ReturnsAsync(new RouteResult
            {
                RoutePath = routePath,
                DistanceInMeters = 5000,
                DurationInSeconds = 600
            });
        _journeyRepoMock.Setup(x => x.AddAsync(It.IsAny<UserJourney>())).ReturnsAsync((UserJourney j) => j);
        _incidentRepoMock.Setup(x => x.GetAlongRouteAsync(It.IsAny<LineString>(), 500))
            .ReturnsAsync(new List<Incident>
            {
                new Incident
                {
                    Id = Guid.NewGuid(),
                    Title = "Pothole",
                    Category = new IncidentCategory { Id = 1, Name = "Pothole" },
                    Location = new Point(28.99, 41.02) { SRID = 4326 },
                    Status = SafeRoad.Core.Enums.IncidentStatus.Pending
                }
            });

        var command = new StartJourneyCommand
        {
            UserId = userId,
            StartLatitude = 41.0082,
            StartLongitude = 28.9784,
            EndLatitude = 41.0432,
            EndLongitude = 29.0090
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.DistanceInKm.Should().Be(5.0);
        result.Data.EstimatedMinutes.Should().Be(10);
        result.Data.IncidentsOnRoute.Should().Be(1);
        result.Data.Incidents.Should().HaveCount(1);
        result.Data.Message.Should().Contain("Warning");
    }

    [Fact]
    public async Task Handle_NoIncidentsOnRoute_ReturnsSafeMessage()
    {
        var userId = Guid.NewGuid();
        var routePath = new LineString(new[]
        {
            new Coordinate(28.9784, 41.0082),
            new Coordinate(29.0090, 41.0432)
        })
        { SRID = 4326 };

        _journeyRepoMock.Setup(x => x.GetActiveByUserIdAsync(userId)).ReturnsAsync((UserJourney?)null);
        _routingServiceMock.Setup(x => x.GetRouteAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
            .ReturnsAsync(new RouteResult
            {
                RoutePath = routePath,
                DistanceInMeters = 3000,
                DurationInSeconds = 300
            });
        _journeyRepoMock.Setup(x => x.AddAsync(It.IsAny<UserJourney>())).ReturnsAsync((UserJourney j) => j);
        _incidentRepoMock.Setup(x => x.GetAlongRouteAsync(It.IsAny<LineString>(), 500))
            .ReturnsAsync(new List<Incident>());

        var command = new StartJourneyCommand
        {
            UserId = userId,
            StartLatitude = 41.0082,
            StartLongitude = 28.9784,
            EndLatitude = 41.0432,
            EndLongitude = 29.0090
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.IncidentsOnRoute.Should().Be(0);
        result.Data.Message.Should().Contain("safe trip");
    }

    [Fact]
    public async Task Handle_ActiveJourneyExists_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        _journeyRepoMock.Setup(x => x.GetActiveByUserIdAsync(userId)).ReturnsAsync(new UserJourney());

        var command = new StartJourneyCommand
        {
            UserId = userId,
            StartLatitude = 41.0082,
            StartLongitude = 28.9784,
            EndLatitude = 41.0432,
            EndLongitude = 29.0090
        };

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
    }

    [Fact]
    public async Task Handle_RoutingFails_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        _journeyRepoMock.Setup(x => x.GetActiveByUserIdAsync(userId)).ReturnsAsync((UserJourney?)null);
        _routingServiceMock.Setup(x => x.GetRouteAsync(It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>(), It.IsAny<double>()))
            .ReturnsAsync((RouteResult?)null);

        var command = new StartJourneyCommand
        {
            UserId = userId,
            StartLatitude = 0,
            StartLongitude = 0,
            EndLatitude = 0,
            EndLongitude = 0
        };

        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
    }
}