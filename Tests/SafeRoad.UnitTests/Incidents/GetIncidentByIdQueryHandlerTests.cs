
using Moq;
using FluentAssertions;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Incidents.Queries.GetIncidentById;

namespace SafeRoad.UnitTests.Incidents;

public class GetIncidentByIdQueryHandlerTests
{
    private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
    private readonly GetIncidentByIdQueryHandler _handler;

    public GetIncidentByIdQueryHandlerTests()
    {
        _handler = new GetIncidentByIdQueryHandler(_incidentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingIncident_ReturnsSuccess()
    {
        var id = Guid.NewGuid();
        var incident = new Incident
        {
            Id = id,
            ReporterUserId = Guid.NewGuid(),
            CategoryId = 1,
            Category = new IncidentCategory { Id = 1, Name = "Pothole" },
            Title = "Test",
            Location = new Point(28.97, 41.00) { SRID = 4326 },
            Status = IncidentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };

        _incidentRepoMock.Setup(x => x.GetWithDetailsAsync(id)).ReturnsAsync(incident);

        var result = await _handler.Handle(new GetIncidentByIdQuery { Id = id }, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.Id.Should().Be(id);
    }

    [Fact]
    public async Task Handle_NonExistentIncident_ThrowsNotFoundException()
    {
        _incidentRepoMock.Setup(x => x.GetWithDetailsAsync(It.IsAny<Guid>())).ReturnsAsync((Incident?)null);

        var act = () => _handler.Handle(new GetIncidentByIdQuery { Id = Guid.NewGuid() }, CancellationToken.None);

        await act.Should().ThrowAsync<NotFoundException>();
    }
}