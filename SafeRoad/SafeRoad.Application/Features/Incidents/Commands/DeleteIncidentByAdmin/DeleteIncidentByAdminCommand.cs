using MediatR;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.DeleteIncidentByAdmin;

public class DeleteIncidentByAdminCommand : IRequest<ApiResponse<string>>
{
    public Guid IncidentId { get; set; }
}
