using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Queries.GetByMunicipality;

public class GetByMunicipalityQuery : IRequest<ApiResponse<List<IncidentResponse>>>
{
    public int MunicipalityId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
