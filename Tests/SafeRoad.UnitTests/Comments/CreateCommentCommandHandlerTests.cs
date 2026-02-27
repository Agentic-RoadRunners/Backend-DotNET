
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Comments.Commands.CreateComment;

namespace SafeRoad.UnitTests.Comments;

public class CreateCommentCommandHandlerTests
{
    private readonly Mock<ICommentRepository> _commentRepoMock = new();
    private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly CreateCommentCommandHandler _handler;

    public CreateCommentCommandHandlerTests()
    {
        _handler = new CreateCommentCommandHandler(_commentRepoMock.Object, _incidentRepoMock.Object, _userRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidComment_ReturnsSuccess()
    {
        var incidentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _incidentRepoMock.Setup(x => x.GetByIdAsync(incidentId)).ReturnsAsync(new Incident { Id = incidentId });
        _userRepoMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(new User { Id = userId, FullName = "Test" });

        var command = new CreateCommentCommand { IncidentId = incidentId, UserId = userId, Content = "Test comment" };
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.Content.Should().Be("Test comment");
    }

    [Fact]
    public async Task Handle_NonExistentIncident_ThrowsNotFound()
    {
        _incidentRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Incident?)null);

        var act = () => _handler.Handle(new CreateCommentCommand { IncidentId = Guid.NewGuid(), UserId = Guid.NewGuid(), Content = "test" }, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}