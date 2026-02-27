
using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQuery : IRequest<ApiResponse<List<IncidentResponse>>>
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
