using MediatR;
using SafeRoad.Core.DTOs.Analytics;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Analytics.Queries.GetCategoryStats;

public class GetCategoryStatsQueryHandler : IRequestHandler<GetCategoryStatsQuery, ApiResponse<List<CategoryStatsResponse>>>
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IIncidentCategoryRepository _categoryRepository;

    public GetCategoryStatsQueryHandler(
        IIncidentRepository incidentRepository,
        IIncidentCategoryRepository categoryRepository)
    {
        _incidentRepository = incidentRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<ApiResponse<List<CategoryStatsResponse>>> Handle(GetCategoryStatsQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllCategoriesAsync();
        var totalIncidents = await _incidentRepository.CountAsync();
        var response = new List<CategoryStatsResponse>();

        foreach (var cat in categories)
        {
            var count = await _incidentRepository.CountAsync(i => i.CategoryId == cat.Id);
            response.Add(new CategoryStatsResponse
            {
                CategoryId = cat.Id,
                CategoryName = cat.Name,
                Count = count,
                Percentage = totalIncidents > 0 ? Math.Round((double)count / totalIncidents * 100, 1) : 0
            });
        }

        return ApiResponse<List<CategoryStatsResponse>>.Success(response);
    }
}
