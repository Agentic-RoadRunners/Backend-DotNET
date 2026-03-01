using MediatR;
using SafeRoad.Core.DTOs.Analytics;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Analytics.Queries.GetTrendData;

public class GetTrendDataQueryHandler : IRequestHandler<GetTrendDataQuery, ApiResponse<List<TrendDataResponse>>>
{
    private readonly IIncidentRepository _incidentRepository;

    public GetTrendDataQueryHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<List<TrendDataResponse>>> Handle(GetTrendDataQuery request, CancellationToken cancellationToken)
    {
        var response = new List<TrendDataResponse>();
        var startDate = DateTime.UtcNow.Date.AddDays(-request.Days + 1);

        for (int i = 0; i < request.Days; i++)
        {
            var date = startDate.AddDays(i);
            var nextDate = date.AddDays(1);

            var count = await _incidentRepository.CountAsync(inc => inc.CreatedAt >= date && inc.CreatedAt < nextDate);
            var resolved = await _incidentRepository.CountAsync(inc => inc.CreatedAt >= date && inc.CreatedAt < nextDate && inc.Status == IncidentStatus.Resolved);

            response.Add(new TrendDataResponse
            {
                Date = date.ToString("yyyy-MM-dd"),
                Count = count,
                Resolved = resolved
            });
        }

        return ApiResponse<List<TrendDataResponse>>.Success(response);
    }
}
