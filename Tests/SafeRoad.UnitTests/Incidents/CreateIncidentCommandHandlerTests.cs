// Tests/SafeRoad.UnitTests/Incidents/CreateIncidentCommandHandlerTests.cs

using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.Incidents.Commands.CreateIncident;

namespace SafeRoad.UnitTests.Incidents;

public class CreateIncidentCommandHandlerTests
{
    private readonly Mock<IIncidentRepository> _incidentRepoMock = new();
    private readonly CreateIncidentCommandHandler _handler;

    public CreateIncidentCommandHandlerTests()
    {
        _handler = new CreateIncidentCommandHandler(_incidentRepoMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsSuccess()
    {
        _incidentRepoMock.Setup(x => x.AddAsync(It.IsAny<Incident>())).ReturnsAsync((Incident i) => i);

        var command = new CreateIncidentCommand
        {
            UserId = Guid.NewGuid(),
            CategoryId = 1,
            Title = "Pothole",
            Description = "Big pothole",
            Latitude = 41.0082,
            Longitude = 28.9784
        };

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Succeeded.Should().BeTrue();
        result.Data!.Title.Should().Be("Pothole");
        result.Data.Status.Should().Be("Pending");
    }
}