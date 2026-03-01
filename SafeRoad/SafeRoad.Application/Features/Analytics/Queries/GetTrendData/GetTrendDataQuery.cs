using MediatR;
using SafeRoad.Core.DTOs.Analytics;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Analytics.Queries.GetTrendData;

public class GetTrendDataQuery : IRequest<ApiResponse<List<TrendDataResponse>>>
{
    public int Days { get; set; } = 30;
}
