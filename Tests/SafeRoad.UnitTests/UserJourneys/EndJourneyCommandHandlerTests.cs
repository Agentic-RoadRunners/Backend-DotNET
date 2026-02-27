using Moq;
using FluentAssertions;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.UserJourneys.Commands.EndJourney;

namespace SafeRoad.UnitTests.UserJourneys;

public class EndJourneyCommandHandlerTests
{
    private readonly Mock<IUserJourneyRepository> _journeyRepoMock = new();
    private readonly EndJourneyCommandHandler _handler;

    public EndJourneyCommandHandlerTests()
    {
        _handler = new EndJourneyCommandHandler(_journeyRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ActiveJourney_ReturnsSuccessWithPrompt()
    {
        var userId = Guid.NewGuid();
        var journey = new UserJourney
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoutePath = new LineString(new[]
            {
                new Coordinate(28.9784, 41.0082),
                new Coordinate(29.0090, 41.0432)
            })
            { SRID = 4326 },
            Status = JourneyStatus.Active,
            CreatedAt = DateTime.UtcNow.AddMinutes(-30)
        };

        _journeyRepoMock.Setup(x => x.GetActiveByUserIdAsync(userId)).ReturnsAsync(journey);

        var result = await _handler.Handle(new EndJourneyCommand { UserId = userId }, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.AskForIncidentReport.Should().BeTrue();
        result.Data.IncidentPrompt.Should().Contain("incident");
        result.Data.DurationMinutes.Should().BeGreaterOrEqualTo(29);
    }

    [Fact]
    public async Task Handle_NoActiveJourney_ThrowsNotFound()
    {
        var userId = Guid.NewGuid();
        _journeyRepoMock.Setup(x => x.GetActiveByUserIdAsync(userId)).ReturnsAsync((UserJourney?)null);

        var act = () => _handler.Handle(new EndJourneyCommand { UserId = userId }, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }

    [Fact]
    public async Task Handle_CompletedJourney_StatusIsCompleted()
    {
        var userId = Guid.NewGuid();
        var journey = new UserJourney
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            RoutePath = new LineString(new[]
            {
                new Coordinate(28.9784, 41.0082),
                new Coordinate(29.0090, 41.0432)
            })
            { SRID = 4326 },
            Status = JourneyStatus.Active,
            CreatedAt = DateTime.UtcNow.AddMinutes(-15)
        };

        _journeyRepoMock.Setup(x => x.GetActiveByUserIdAsync(userId)).ReturnsAsync(journey);

        await _handler.Handle(new EndJourneyCommand { UserId = userId }, CancellationToken.None);

        journey.Status.Should().Be(JourneyStatus.Completed);
    }
}