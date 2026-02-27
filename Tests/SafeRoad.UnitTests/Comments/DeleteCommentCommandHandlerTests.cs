
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Comments.Commands.DeleteComment;

namespace SafeRoad.UnitTests.Comments;

public class DeleteCommentCommandHandlerTests
{
    private readonly Mock<ICommentRepository> _commentRepoMock = new();
    private readonly DeleteCommentCommandHandler _handler;

    public DeleteCommentCommandHandlerTests()
    {
        _handler = new DeleteCommentCommandHandler(_commentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_OwnComment_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var comment = new Comment { Id = 1, UserId = userId, Content = "test" };
        _commentRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(comment);

        var result = await _handler.Handle(new DeleteCommentCommand { CommentId = 1, UserId = userId }, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_OtherUsersComment_ThrowsForbidden()
    {
        var comment = new Comment { Id = 1, UserId = Guid.NewGuid(), Content = "test" };
        _commentRepoMock.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(comment);

        var act = () => _handler.Handle(new DeleteCommentCommand { CommentId = 1, UserId = Guid.NewGuid() }, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
    }
}