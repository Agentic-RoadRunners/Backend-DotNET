
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.WatchedAreas.Commands.CreateWatchedArea;

namespace SafeRoad.UnitTests.WatchedAreas;

public class CreateWatchedAreaCommandHandlerTests
{
    private readonly Mock<IWatchedAreaRepository> _watchedAreaRepoMock = new();
    private readonly CreateWatchedAreaCommandHandler _handler;

    public CreateWatchedAreaCommandHandlerTests()
    {
        _handler = new CreateWatchedAreaCommandHandler(_watchedAreaRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        _watchedAreaRepoMock.Setup(x => x.AddAsync(It.IsAny<WatchedArea>())).ReturnsAsync((WatchedArea w) => w);

        var command = new CreateWatchedAreaCommand
        {
            UserId = Guid.NewGuid(),
            Label = "Home",
            Latitude = 41.0082,
            Longitude = 28.9784,
            RadiusInMeters = 5000
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.Label.Should().Be("Home");
    }
}