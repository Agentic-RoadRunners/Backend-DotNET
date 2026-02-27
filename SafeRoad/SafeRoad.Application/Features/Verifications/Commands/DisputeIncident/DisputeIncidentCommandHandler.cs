using MediatR;
using SafeRoad.Core.Entities;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Verifications.Commands.DisputeIncident;

public class DisputeIncidentCommandHandler : IRequestHandler<DisputeIncidentCommand, ApiResponse<string>>
{
    private readonly IVerificationRepository _verificationRepository;
    private readonly IIncidentRepository _incidentRepository;

    public DisputeIncidentCommandHandler(IVerificationRepository verificationRepository, IIncidentRepository incidentRepository)
    {
        _verificationRepository = verificationRepository;
        _incidentRepository = incidentRepository;
    }

    public async Task<ApiResponse<string>> Handle(DisputeIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await _incidentRepository.GetByIdAsync(request.IncidentId);
        if (incident == null)
            throw new NotFoundException("Incident", request.IncidentId);

        if (incident.ReporterUserId == request.UserId)
            throw new ForbiddenException("You cannot dispute your own incident.");

        var existing = await _verificationRepository.GetByUserAndIncidentAsync(request.UserId, request.IncidentId);
        if (existing != null)
            throw new BadRequestException("You have already voted on this incident.");

        var verification = new Verification
        {
            IncidentId = request.IncidentId,
            UserId = request.UserId,
            IsPositive = false,
            CreatedAt = DateTime.UtcNow
        };

        await _verificationRepository.AddAsync(verification);

        return ApiResponse<string>.Success("Incident disputed successfully.");
    }
}