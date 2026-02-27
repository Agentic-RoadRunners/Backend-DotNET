
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Interfaces.Services;
using SafeRoad.Core.Features.Auth.Commands.Login;
using SafeRoad.Core.Exceptions;
namespace SafeRoad.UnitTests.Auth;

public class LoginCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _handler = new LoginCommandHandler(_userRepoMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCredentials_ReturnsSuccess()
    {
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword("Test1234!");
        var user = new User { Id = Guid.NewGuid(), Email = "test@test.com", PasswordHash = hashedPassword, FullName = "Test" };

        _userRepoMock.Setup(x => x.GetByEmailAsync("test@test.com")).ReturnsAsync(user);
        _userRepoMock.Setup(x => x.GetWithRolesAsync(user.Id)).ReturnsAsync(user);
        _tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<User>(), It.IsAny<IList<string>>())).Returns("fake-token");

        var command = new LoginCommand { Email = "test@test.com", Password = "Test1234!" };
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.AccessToken.Should().Be("fake-token");
    }

    [Fact]
    public async Task Handle_WrongPassword_ThrowsUnauthorized()
    {
        var user = new User { Email = "test@test.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct") };
        _userRepoMock.Setup(x => x.GetByEmailAsync("test@test.com")).ReturnsAsync(user);

        var command = new LoginCommand { Email = "test@test.com", Password = "wrong" };
        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
    }

    [Fact]
    public async Task Handle_NonExistentEmail_ThrowsUnauthorized()
    {
        _userRepoMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);

        var command = new LoginCommand { Email = "noone@test.com", Password = "Test1234!" };
        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<UnauthorizedException>();
    }
}