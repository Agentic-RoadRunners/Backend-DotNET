using MediatR;
using SafeRoad.Core.DTOs.IncidentCategory;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.IncidentCategories.Queries.GetAllIncidentCategories;

public class GetAllIncidentCategoriesQueryHandler
    : IRequestHandler<GetAllIncidentCategoriesQuery, ApiResponse<List<IncidentCategoryResponse>>>
{
    private readonly IIncidentCategoryRepository _categoryRepository;

    public GetAllIncidentCategoriesQueryHandler(IIncidentCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ApiResponse<List<IncidentCategoryResponse>>> Handle(
        GetAllIncidentCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();

        var response = categories.Select(c => new IncidentCategoryResponse
        {
            Id = c.Id,
            Name = c.Name
        }).ToList();

        return ApiResponse<List<IncidentCategoryResponse>>.Success(response);
    }
}