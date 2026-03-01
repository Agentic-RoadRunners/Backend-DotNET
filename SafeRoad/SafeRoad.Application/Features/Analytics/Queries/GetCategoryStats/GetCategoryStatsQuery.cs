using MediatR;
using SafeRoad.Core.DTOs.Analytics;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Analytics.Queries.GetCategoryStats;

public class GetCategoryStatsQuery : IRequest<ApiResponse<List<CategoryStatsResponse>>>
{
}
