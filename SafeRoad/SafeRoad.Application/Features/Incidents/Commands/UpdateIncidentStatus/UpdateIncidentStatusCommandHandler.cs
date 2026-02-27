
using MediatR;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Incidents.Commands.UpdateIncidentStatus;

public class UpdateIncidentStatusCommandHandler : IRequestHandler<UpdateIncidentStatusCommand, ApiResponse<string>>
{
    private readonly IIncidentRepository _incidentRepository;

    public UpdateIncidentStatusCommandHandler(IIncidentRepository incidentRepository)
    {
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<string>> Handle(UpdateIncidentStatusCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);
        if (incident == null)
            throw new NotFoundException("Incident", request.IncidentId);

        incident.Status = request.NewStatus;
        await _incidentRepository.UpdateAsync(incident);

        return ApiResponse<string>.Success($"Status updated to {request.NewStatus}.");
    }
}