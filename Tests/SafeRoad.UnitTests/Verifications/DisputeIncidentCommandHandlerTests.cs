
using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Verifications.Commands.DisputeIncident;

namespace SafeRoad.UnitTests.Verifications;

public class DisputeIncidentCommandHandlerTests
{
    private readonly Mock<IVerificationRepository> _verifyRepoMock = new();
    private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
    private readonly DisputeIncidentCommandHandler _handler;

    public DisputeIncidentCommandHandlerTests()
    {
        _handler = new DisputeIncidentCommandHandler(_verifyRepoMock.Object, _incidentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidDispute_ReturnsSuccess()
    {
        var incidentId = Guid.NewGuid();
        var userId = Guid.NewGuid();
        _incidentRepoMock.Setup(x => x.GetByIdAsync(incidentId)).ReturnsAsync(new Incident { Id = incidentId, ReporterUserId = Guid.NewGuid() });
        _verifyRepoMock.Setup(x => x.GetByUserAndIncidentAsync(userId, incidentId)).ReturnsAsync((Verification?)null);

        var result = await _handler.Handle(new DisputeIncidentCommand { IncidentId = incidentId, UserId = userId }, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_NonExistentIncident_ThrowsNotFound()
    {
        _incidentRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Incident?)null);

        var act = () => _handler.Handle(new DisputeIncidentCommand { IncidentId = Guid.NewGuid(), UserId = Guid.NewGuid() }, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}