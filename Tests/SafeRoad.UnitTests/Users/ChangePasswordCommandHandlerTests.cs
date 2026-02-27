
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Users.Commands.ChangePassword;

namespace SafeRoad.UnitTests.Users;

public class ChangePasswordCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly ChangePasswordCommandHandler _handler;

    public ChangePasswordCommandHandlerTests()
    {
        _handler = new ChangePasswordCommandHandler(_userRepoMock.Object);
    }

    [Fact]
    public async Task Handle_CorrectCurrentPassword_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, PasswordHash = BCrypt.Net.BCrypt.HashPassword("OldPass123!") };
        _userRepoMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

        var command = new ChangePasswordCommand { UserId = userId, CurrentPassword = "OldPass123!", NewPassword = "NewPass456!" };
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_WrongCurrentPassword_ThrowsBadRequest()
    {
        var userId = Guid.NewGuid();
        var user = new User { Id = userId, PasswordHash = BCrypt.Net.BCrypt.HashPassword("OldPass123!") };
        _userRepoMock.Setup(x => x.GetByIdAsync(userId)).ReturnsAsync(user);

        var command = new ChangePasswordCommand { UserId = userId, CurrentPassword = "WrongPass!", NewPassword = "NewPass456!" };
        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
    }
}