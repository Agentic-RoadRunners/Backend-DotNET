using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Features.Users.Commands.ChangePassword;
using SafeRoad.Core.Features.Users.Commands.UpdateProfile;
using SafeRoad.Core.Features.Users.Queries.GetMyProfile;
using SafeRoad.Core.Features.Users.Queries.GetPublicProfile;
using SafeRoad.Core.Features.Users.Queries.GetTrustScore;
using SafeRoad.Core.Features.Users.Queries.GetUserStats;
using SafeRoad.WebApi.Services;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// User profile management — view, update profile, change password, and statistics
/// </summary>
[ApiController]
[Route("api/users")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public UserController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Get your own profile
    /// </summary>
    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> GetMyProfile()
    {
        var result = await _mediator.Send(new GetMyProfileQuery { UserId = _currentUser.UserId!.Value });
        return Ok(result);
    }

    /// <summary>
    /// Update your profile (name, avatar)
    /// </summary>
    [HttpPut("me")]
    [Authorize]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var result = await _mediator.Send(new UpdateProfileCommand
        {
            UserId = _currentUser.UserId!.Value,
            FullName = request.FullName,
            AvatarUrl = request.AvatarUrl
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Change your password
    /// </summary>
    [HttpPut("me/password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var result = await _mediator.Send(new ChangePasswordCommand
        {
            UserId = _currentUser.UserId!.Value,
            CurrentPassword = request.CurrentPassword,
            NewPassword = request.NewPassword
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Get your personal statistics (total incidents, comments, verifications, trust score)
    /// </summary>
    [HttpGet("me/stats")]
    [Authorize]
    public async Task<IActionResult> GetMyStats()
    {
        var result = await _mediator.Send(new GetUserStatsQuery { UserId = _currentUser.UserId!.Value });
        return Ok(result);
    }

    /// <summary>
    /// Get a user's public profile
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetPublicProfile(Guid id)
    {
        var result = await _mediator.Send(new GetPublicProfileQuery { UserId = id });
        if (!result.Succeeded) return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Get a user's trust score
    /// </summary>
    [HttpGet("{id:guid}/trust-score")]
    public async Task<IActionResult> GetTrustScore(Guid id)
    {
        var result = await _mediator.Send(new GetTrustScoreQuery { UserId = id });
        if (!result.Succeeded) return NotFound(result);
        return Ok(result);
    }
}