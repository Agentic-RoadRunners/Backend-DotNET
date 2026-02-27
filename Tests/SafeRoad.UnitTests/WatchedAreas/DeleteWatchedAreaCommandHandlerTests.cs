
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.WatchedAreas.Commands.DeleteWatchedArea;

namespace SafeRoad.UnitTests.WatchedAreas;

public class DeleteWatchedAreaCommandHandlerTests
{
    private readonly Mock<IWatchedAreaRepository> _watchedAreaRepoMock = new();
    private readonly DeleteWatchedAreaCommandHandler _handler;

    public DeleteWatchedAreaCommandHandlerTests()
    {
        _handler = new DeleteWatchedAreaCommandHandler(_watchedAreaRepoMock.Object);
    }

    [Fact]
    public async Task Handle_OwnArea_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var area = new WatchedArea { Id = 1, UserId = userId };
        _watchedAreaRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(area);

        var result = await _handler.Handle(new DeleteWatchedAreaCommand { Id = 1, UserId = userId }, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_OtherUsersArea_ThrowsForbidden()
    {
        var area = new WatchedArea { Id = 1, UserId = Guid.NewGuid() };
        _watchedAreaRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(area);

        var act = () => _handler.Handle(new DeleteWatchedAreaCommand { Id = 1, UserId = Guid.NewGuid() }, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Handle_NonExistent_ThrowsNotFound()
    {
        _watchedAreaRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((WatchedArea?)null);

        var act = () => _handler.Handle(new DeleteWatchedAreaCommand { Id = 999, UserId = Guid.NewGuid() }, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}