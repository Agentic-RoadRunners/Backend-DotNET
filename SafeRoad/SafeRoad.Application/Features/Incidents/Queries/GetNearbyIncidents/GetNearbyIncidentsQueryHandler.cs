    
using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetNearbyIncidents;

public class GetNearbyIncidentsQueryHandler : IRequestHandler<GetNearbyIncidentsQuery, ApiResponse<List<IncidentResponse>>>
{
    private readonly IIncidentRepository _incidentRepository;

    public GetNearbyIncidentsQueryHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<List<IncidentResponse>>> Handle(GetNearbyIncidentsQuery request, CancellationToken cancellationToken)
    {
        var incidents = await _incidentRepository.GetNearbyAsync(
            request.Latitude, request.Longitude, request.RadiusMeters, request.Page, request.PageSize);

        var response = incidents.Select(i => new IncidentResponse
        {
            Id = i.Id,
            ReporterUserId = i.ReporterUserId,
            ReporterName = i.Reporter?.FullName,
            CategoryId = i.CategoryId,
            CategoryName = i.Category.Name,
            Title = i.Title,
            Description = i.Description,
            Latitude = i.Location.Y,
            Longitude = i.Location.X,
            Status = i.Status.ToString(),
            CreatedAt = i.CreatedAt
        }).ToList();

        return ApiResponse<List<IncidentResponse>>.Success(response);
    }
}