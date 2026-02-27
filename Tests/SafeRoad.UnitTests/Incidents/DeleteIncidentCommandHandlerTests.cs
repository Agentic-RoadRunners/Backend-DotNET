using Moq;
using FluentAssertions;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Incidents.Commands.DeleteIncident;

namespace SafeRoad.UnitTests.Incidents;

public class DeleteIncidentCommandHandlerTests
{
    private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
    private readonly DeleteIncidentCommandHandler _handler;

    public DeleteIncidentCommandHandlerTests()
    {
        _handler = new DeleteIncidentCommandHandler(_incidentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_OwnIncident_ReturnsSuccess()
    {
        var userId = Guid.NewGuid();
        var incident = new Incident { Id = Guid.NewGuid(), ReporterUserId = userId };
        _incidentRepoMock.Setup(x => x.GetByIdAsync(incident.Id)).ReturnsAsync(incident);

        var result = await _handler.Handle(new DeleteIncidentCommand { IncidentId = incident.Id, UserId = userId }, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_OtherUsersIncident_ThrowsForbidden()
    {
        var incident = new Incident { Id = Guid.NewGuid(), ReporterUserId = Guid.NewGuid() };
        _incidentRepoMock.Setup(x => x.GetByIdAsync(incident.Id)).ReturnsAsync(incident);

        var act = () => _handler.Handle(new DeleteIncidentCommand { IncidentId = incident.Id, UserId = Guid.NewGuid() }, CancellationToken.None);

        await act.Should().ThrowAsync<ForbiddenException>();
    }

    [Fact]
    public async Task Handle_NonExistentIncident_ThrowsNotFound()
    {
        _incidentRepoMock.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync((Incident?)null);

        var act = () => _handler.Handle(new DeleteIncidentCommand { IncidentId = Guid.NewGuid(), UserId = Guid.NewGuid() }, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}