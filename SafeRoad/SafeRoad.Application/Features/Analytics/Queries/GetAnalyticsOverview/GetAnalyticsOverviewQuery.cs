using MediatR;
using SafeRoad.Core.DTOs.Analytics;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Analytics.Queries.GetAnalyticsOverview;

public class GetAnalyticsOverviewQuery : IRequest<ApiResponse<AnalyticsOverviewResponse>>
{
}
