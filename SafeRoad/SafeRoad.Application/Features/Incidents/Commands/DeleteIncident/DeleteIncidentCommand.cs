
using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommand : IRequest<ApiResponse<string>>
{
    public Guid IncidentId { get; set; }
    public Guid UserId { get; set; }
}