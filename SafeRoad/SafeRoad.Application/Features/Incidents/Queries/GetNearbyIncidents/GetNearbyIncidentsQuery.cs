
using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetNearbyIncidents;

public class GetNearbyIncidentsQuery : IRequest<ApiResponse<List<IncidentResponse>>>
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public int RadiusMeters { get; set; } = 5000;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}