
using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQuery : IRequest<ApiResponse<IncidentDetailResponse>>
{
    public Guid Id { get; set; }
}