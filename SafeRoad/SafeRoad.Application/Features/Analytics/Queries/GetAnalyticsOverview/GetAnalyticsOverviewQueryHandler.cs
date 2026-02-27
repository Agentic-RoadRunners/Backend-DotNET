using MediatR;
using SafeRoad.Core.DTOs.Analytics;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Enums;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Analytics.Queries.GetAnalyticsOverview;

public class GetAnalyticsOverviewQueryHandler
    : IRequestHandler<GetAnalyticsOverviewQuery, ApiResponse<AnalyticsOverviewResponse>>
{
    private readonly IIncidentRepository _incidentRepository;
    private readonly IUserRepository _userRepository;
    private readonly IGenericRepository<Municipality> _municipalityRepository;

    public GetAnalyticsOverviewQueryHandler(
        IIncidentRepository incidentRepository,
        IUserRepository userRepository,
        IGenericRepository<Municipality> municipalityRepository)
    {
        _incidentRepository = incidentRepository;
        _userRepository = userRepository;
        _municipalityRepository = municipalityRepository;
    }

    public async Task<ApiResponse<AnalyticsOverviewResponse>> Handle(
        GetAnalyticsOverviewQuery request, CancellationToken cancellationToken)
    {
        var totalIncidents = await _incidentRepository.CountAsync();
        var totalUsers = await _userRepository.CountAsync();
        var resolvedCount = await _incidentRepository.CountAsync(i => i.Status == IncidentStatus.Resolved);
        var municipalityCount = await _municipalityRepository.CountAsync();

        var response = new AnalyticsOverviewResponse
        {
            TotalIncidents = totalIncidents,
            TotalUsers = totalUsers,
            ResolvedCount = resolvedCount,
            MunicipalityCount = municipalityCount
        };

        return ApiResponse<AnalyticsOverviewResponse>.Success(response);
    }
}
