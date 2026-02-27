using MediatR;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Verifications.Commands.RemoveVerification;

public class RemoveVerificationCommandHandler : IRequestHandler<RemoveVerificationCommand, ApiResponse<string>>
{
    private readonly IVerificationRepository _verificationRepository;

    public RemoveVerificationCommandHandler(IVerificationRepository verificationRepository)
    {
        _verificationRepository = verificationRepository;
    }

    public async Task<ApiResponse<string>> Handle(RemoveVerificationCommand request, CancellationToken cancellationToken)
    {
        var verification = await _verificationRepository.GetByUserAndIncidentAsync(request.UserId, request.IncidentId);
        if (verification == null)
            throw new NotFoundException("Verification", $"{request.UserId}/{request.IncidentId}");

        await _verificationRepository.DeleteAsync(verification);

        return ApiResponse<string>.Success("Vote removed successfully.");
    }
}