
using Moq;
using FluentAssertions;
using NetTopologySuite.Geometries;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Incidents.Queries.GetAllIncidents;

namespace SafeRoad.UnitTests.Incidents;

public class GetAllIncidentsQueryHandlerTests
{
    private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
    private readonly GetAllIncidentsQueryHandler _handler;

    public GetAllIncidentsQueryHandlerTests()
    {
        _handler = new GetAllIncidentsQueryHandler(_incidentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsAllIncidents_Successfully()
    {
        var incidents = new List<Incident>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ReporterUserId = Guid.NewGuid(),
                CategoryId = 1,
                Category = new IncidentCategory { Id = 1, Name = "Pothole" },
                Title = "Big pothole",
                Description = "On main road",
                Location = new Point(28.97, 41.00) { SRID = 4326 },
                Status = IncidentStatus.Pending,
                CreatedAt = DateTime.UtcNow
            },
            new()
            {
                Id = Guid.NewGuid(),
                ReporterUserId = Guid.NewGuid(),
                CategoryId = 2,
                Category = new IncidentCategory { Id = 2, Name = "Traffic Light" },
                Title = "Broken traffic light",
                Description = "Not working",
                Location = new Point(29.00, 41.01) { SRID = 4326 },
                Status = IncidentStatus.Verified,
                CreatedAt = DateTime.UtcNow
            }
        };

        _incidentRepoMock
            .Setup(x => x.GetAllPaginatedAsync(1, 20))
            .ReturnsAsync(incidents);

        var result = await _handler.Handle(
            new GetAllIncidentsQuery { Page = 1, PageSize = 20 },
            CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data.Should().HaveCount(2);
        result.Data![0].Title.Should().Be("Big pothole");
        result.Data[1].Title.Should().Be("Broken traffic light");
    }

    [Fact]
    public async Task Handle_EmptyList_ReturnsEmptySuccess()
    {
        _incidentRepoMock
            .Setup(x => x.GetAllPaginatedAsync(1, 20))
            .ReturnsAsync(new List<Incident>());

        var result = await _handler.Handle(
            new GetAllIncidentsQuery { Page = 1, PageSize = 20 },
            CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_RespectsPageParameters()
    {
        _incidentRepoMock
            .Setup(x => x.GetAllPaginatedAsync(2, 10))
            .ReturnsAsync(new List<Incident>());

        var result = await _handler.Handle(
            new GetAllIncidentsQuery { Page = 2, PageSize = 10 },
            CancellationToken.None);

        _incidentRepoMock.Verify(x => x.GetAllPaginatedAsync(2, 10), Times.Once);
        result.Succeeded.Should().BeTrue();
    }
}
