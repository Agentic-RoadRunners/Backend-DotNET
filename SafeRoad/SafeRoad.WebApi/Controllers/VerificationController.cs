using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.Features.Verifications.Commands.VerifyIncident;
using SafeRoad.Core.Features.Verifications.Commands.DisputeIncident;
using SafeRoad.Core.Features.Verifications.Commands.RemoveVerification;
using SafeRoad.Core.Features.Verifications.Queries.GetVerifications;
using SafeRoad.WebApi.Services;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// Incident verification — verify, dispute, and manage votes
/// </summary>
[ApiController]
[Route("api/incidents/{incidentId:guid}")]
[Produces("application/json")]
public class VerificationController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public VerificationController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Verify an incident (upvote)
    /// </summary>
    [HttpPost("verify")]
    [Authorize]
    public async Task<IActionResult> Verify(Guid incidentId)
    {
        var result = await _mediator.Send(new VerifyIncidentCommand
        {
            IncidentId = incidentId,
            UserId = _currentUser.UserId!.Value
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Dispute an incident (downvote)
    /// </summary>
    [HttpPost("dispute")]
    [Authorize]
    public async Task<IActionResult> Dispute(Guid incidentId)
    {
        var result = await _mediator.Send(new DisputeIncidentCommand
        {
            IncidentId = incidentId,
            UserId = _currentUser.UserId!.Value
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Remove your vote from an incident
    /// </summary>
    [HttpDelete("verify")]
    [Authorize]
    public async Task<IActionResult> RemoveVote(Guid incidentId)
    {
        var result = await _mediator.Send(new RemoveVerificationCommand
        {
            IncidentId = incidentId,
            UserId = _currentUser.UserId!.Value
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Get all verifications for an incident
    /// </summary>
    [HttpGet("verifications")]
    public async Task<IActionResult> GetVerifications(Guid incidentId)
    {
        var result = await _mediator.Send(new GetVerificationsQuery { IncidentId = incidentId });
        return Ok(result);
    }
}