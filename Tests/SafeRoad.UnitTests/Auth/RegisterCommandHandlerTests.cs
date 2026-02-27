
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Interfaces.Services;
using SafeRoad.Core.Features.Auth.Commands.Register;
using SafeRoad.Core.Exceptions;
namespace SafeRoad.UnitTests.Auth;

public class RegisterCommandHandlerTests
{
    private readonly Mock<IUserRepository> _userRepoMock = new();
    private readonly Mock<ITokenService> _tokenServiceMock = new();
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTests()
    {
        _handler = new RegisterCommandHandler(_userRepoMock.Object, _tokenServiceMock.Object);
    }

    [Fact]
    public async Task Handle_NewUser_ReturnsSuccess()
    {
        _userRepoMock.Setup(x => x.GetByEmailAsync(It.IsAny<string>())).ReturnsAsync((User?)null);
        _tokenServiceMock.Setup(x => x.GenerateAccessToken(It.IsAny<User>(), It.IsAny<IList<string>>())).Returns("fake-token");

        var command = new RegisterCommand { Email = "test@test.com", Password = "Test1234!", FullName = "Test User" };
        var result = await _handler.Handle(command, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.AccessToken.Should().Be("fake-token");
        result.Data.Email.Should().Be("test@test.com");
    }

    [Fact]
    public async Task Handle_ExistingEmail_ThrowsBadRequest()
    {
        _userRepoMock.Setup(x => x.GetByEmailAsync("test@test.com")).ReturnsAsync(new User { Email = "test@test.com" });

        var command = new RegisterCommand { Email = "test@test.com", Password = "Test1234!" };
        var act = () => _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
    }
}