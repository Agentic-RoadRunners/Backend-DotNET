using MediatR;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.UpdateIncidentByAdmin;

public class UpdateIncidentByAdminCommand : IRequest<ApiResponse<IncidentResponse>>
{
    public Guid IncidentId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? Status { get; set; }
}
