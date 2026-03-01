using MediatR;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.DeleteIncidentByAdmin;

public class DeleteIncidentByAdminCommandHandler : IRequestHandler<DeleteIncidentByAdminCommand, ApiResponse<string>>
{
    private readonly IIncidentRepository _incidentRepository;

    public DeleteIncidentByAdminCommandHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<string>> Handle(DeleteIncidentByAdminCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);
        if (incident == null)
            throw new NotFoundException("Incident", request.IncidentId);

        await _incidentRepository.DeleteAsync(incident);

        return ApiResponse<string>.Success("Incident deleted successfully.");
    }
}
