
using MediatR;
using SafeRoad.Core.Enums;
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
        if (!Enum.TryParse<IncidentStatus>(request.NewStatus, true, out var parsedStatus))
            throw new BadRequestException($"Invalid status value '{request.NewStatus}'. Valid values: {string.Join(", ", Enum.GetNames<IncidentStatus>())}");

        var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);
        if (incident == null)
            throw new NotFoundException("Incident", request.IncidentId);

        incident.Status = parsedStatus;
        await _incidentRepository.UpdateAsync(incident);

        return ApiResponse<string>.Success($"Status updated to {parsedStatus}.");
    }
}