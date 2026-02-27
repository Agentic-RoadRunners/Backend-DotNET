
using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetMyIncidents;

public class GetMyIncidentsQuery : IRequest<ApiResponse<List<IncidentResponse>>>
{
    public Guid UserId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}