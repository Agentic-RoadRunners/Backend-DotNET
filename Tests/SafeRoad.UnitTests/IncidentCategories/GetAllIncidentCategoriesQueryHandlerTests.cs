using Moq;
using FluentAssertions;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Features.IncidentCategories.Queries.GetAllIncidentCategories;

namespace SafeRoad.UnitTests.IncidentCategories;

public class GetAllIncidentCategoriesQueryHandlerTests
{
    private readonly Mock<IIncidentCategoryRepository> _categoryRepoMock = new();
    private readonly GetAllIncidentCategoriesQueryHandler _handler;

    public GetAllIncidentCategoriesQueryHandlerTests()
    {
        _handler = new GetAllIncidentCategoriesQueryHandler(_categoryRepoMock.Object);
    }

    [Fact]
    public async Task Handle_WithCategories_ReturnsAllCategories()
    {
        // Arrange
        var categories = new List<IncidentCategory>
        {
            new() { Id = 1, Name = "Traffic Accident" },
            new() { Id = 2, Name = "Pothole" },
            new() { Id = 3, Name = "Road Damage" }
        };

        _categoryRepoMock
            .Setup(x => x.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _handler.Handle(new GetAllIncidentCategoriesQuery(), CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data.Should().HaveCount(3);
        result.Data![0].Id.Should().Be(1);
        result.Data[0].Name.Should().Be("Traffic Accident");
        result.Data[1].Name.Should().Be("Pothole");
        result.Data[2].Name.Should().Be("Road Damage");
    }

    [Fact]
    public async Task Handle_EmptyDatabase_ReturnsEmptyList()
    {
        // Arrange
        _categoryRepoMock
            .Setup(x => x.GetAllCategoriesAsync())
            .ReturnsAsync(new List<IncidentCategory>());

        // Act
        var result = await _handler.Handle(new GetAllIncidentCategoriesQuery(), CancellationToken.None);

        // Assert
        result.Succeeded.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data.Should().BeEmpty();
    }

    [Fact]
    public async Task Handle_ReturnsCorrectMappedFields()
    {
        // Arrange
        var categories = new List<IncidentCategory>
        {
            new() { Id = 7, Name = "Street Light Out" }
        };

        _categoryRepoMock
            .Setup(x => x.GetAllCategoriesAsync())
            .ReturnsAsync(categories);

        // Act
        var result = await _handler.Handle(new GetAllIncidentCategoriesQuery(), CancellationToken.None);

        // Assert
        result.Data.Should().HaveCount(1);
        result.Data![0].Id.Should().Be(7);
        result.Data[0].Name.Should().Be("Street Light Out");
    }

    [Fact]
    public async Task Handle_CallsRepositoryOnce()
    {
        // Arrange
        _categoryRepoMock
            .Setup(x => x.GetAllCategoriesAsync())
            .ReturnsAsync(new List<IncidentCategory>());

        // Act
        await _handler.Handle(new GetAllIncidentCategoriesQuery(), CancellationToken.None);

        // Assert
        _categoryRepoMock.Verify(x => x.GetAllCategoriesAsync(), Times.Once);
    }
}
