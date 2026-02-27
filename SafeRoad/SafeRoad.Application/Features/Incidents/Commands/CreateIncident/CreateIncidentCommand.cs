
using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.CreateIncident;

public class CreateIncidentCommand : IRequest<ApiResponse<IncidentResponse>>
{
    public Guid UserId { get; set; }
    public int CategoryId { get; set; }
    public int? MunicipalityId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
}