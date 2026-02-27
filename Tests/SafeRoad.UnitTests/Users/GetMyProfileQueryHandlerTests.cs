
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Users.Queries.GetMyProfile;

namespace SafeRoad.UnitTests.Users;

public class GetMyProfileQueryHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly GetMyProfileQueryHandler _handler;

    public GetMyProfileQueryHandlerTests()
    {
        _handler = new GetMyProfileQueryHandler(_userRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingUser_ReturnsProfile()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, Email = "test@test.com", FullName = "Test", TrustScore = 50, CreatedAt = DateTime.UtcNow };
        _userRepoMock.Setup(x => x.GetWithRolesAsync(userId)).ReturnsAsync(user);

        var result = await _handler.Handle(new GetMyProfileQuery { UserId = userId }, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task Handle_NonExistentUser_ThrowsNotFound()
    {
        _userRepoMock.Setup(x => x.GetWithRolesAsync(It.IsAny<Guid>())).ReturnsAsync((User?)null);

        var act = () => _handler.Handle(new GetMyProfileQuery { UserId = Guid.NewGuid() }, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}