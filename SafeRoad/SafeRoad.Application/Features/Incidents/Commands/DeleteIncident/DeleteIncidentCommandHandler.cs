
using MediatR;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommandHandler : IRequestHandler<DeleteIncidentCommand, ApiResponse<string>>
{
    private readonly IIncidentRepository _incidentRepository;

    public DeleteIncidentCommandHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<string>> Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);
        if (incident == null)
            throw new NotFoundException("Incident", request.IncidentId);

        if (incident.ReporterUserId != request.UserId)
            throw new ForbiddenException("You can only delete your own incidents.");

        await _incidentRepository.DeleteAsync(incident);

        return ApiResponse<string>.Success("Incident deleted successfully.");
    }
}