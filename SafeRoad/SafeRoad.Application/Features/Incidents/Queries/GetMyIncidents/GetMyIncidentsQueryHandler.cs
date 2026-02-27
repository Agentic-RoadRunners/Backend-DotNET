
using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetMyIncidents;

public class GetMyIncidentsQueryHandler : IRequestHandler<GetMyIncidentsQuery, ApiResponse<List<IncidentResponse>>>
{
    private readonly IIncidentRepository _incidentRepository;

    public GetMyIncidentsQueryHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<List<IncidentResponse>>> Handle(GetMyIncidentsQuery request, CancellationToken cancellationToken)
    {
        var incidents = await _incidentRepository.GetByUserIdAsync(request.UserId, request.Page, request.PageSize);

        var response = incidents.Select(i => new IncidentResponse
        {
            Id = i.Id,
            ReporterUserId = i.ReporterUserId,
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