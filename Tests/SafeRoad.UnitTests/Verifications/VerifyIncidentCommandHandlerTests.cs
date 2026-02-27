
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Verifications.Commands.VerifyIncident;
namespace SafeRoad.UnitTests.Verifications;

public class VerifyIncidentCommandHandlerTests
{
    private readonly Mock<IVerificationRepository> _verifyRepoMock = new();
    private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
    private readonly VerifyIncidentCommandHandler _handler;

    public VerifyIncidentCommandHandlerTests()
    {
        _handler = new VerifyIncidentCommandHandler(_verifyRepoMock.Object, _incidentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidVerify_ReturnsSuccess()
    {
        var incidentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _incidentRepoMock.Setup(x => x.GetByIdAsync(incidentId)).ReturnsAsync(new Incident { Id = incidentId, ReporterUserId = Guid.NewGuid() });
        _verifyRepoMock.Setup(x => x.GetByUserAndIncidentAsync(userId, incidentId)).ReturnsAsync((Verification?)null);

        var result = await _handler.Handle(new VerifyIncidentCommand { IncidentId = incidentId, UserId = userId }, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_OwnIncident_ThrowsForbidden()
    {
        var userId = Guid.NewGuid();
        _incidentRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(new Incident { ReporterUserId = userId });

        var act = () => _handler.Handle(new VerifyIncidentCommand { IncidentId = Guid.NewGuid(), UserId = userId }, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Handle_AlreadyVoted_ThrowsBadRequest()
    {
        var incidentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _incidentRepoMock.Setup(x => x.GetByIdAsync(incidentId)).ReturnsAsync(new Incident { Id = incidentId, ReporterUserId = Guid.NewGuid() });
        _verifyRepoMock.Setup(x => x.GetByUserAndIncidentAsync(userId, incidentId)).ReturnsAsync(new Verification());

        var act = () => _handler.Handle(new VerifyIncidentCommand { IncidentId = incidentId, UserId = userId }, CancellationToken.None);

        await act.Should().ThrowAsync<BadRequestException>();
    }
}