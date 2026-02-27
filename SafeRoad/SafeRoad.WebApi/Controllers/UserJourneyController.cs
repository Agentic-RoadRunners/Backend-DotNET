using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.DTOs.UserJourney;
using SafeRoad.Core.Features.UserJourneys.Commands.StartJourney;
using SafeRoad.Core.Features.UserJourneys.Commands.EndJourney;
using SafeRoad.Core.Features.UserJourneys.Queries.GetActiveJourney;
using SafeRoad.Core.Features.UserJourneys.Queries.GetMyJourneys;
using SafeRoad.WebApi.Services;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// User journeys — start a trip with route calculation and incident warnings, end trip with report prompt
/// </summary>
[ApiController]
[Route("api/journeys")]
[Produces("application/json")]
[Authorize]
public class UserJourneyController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public UserJourneyController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Start a new journey — calculates route and returns incidents along the way
    /// </summary>
    [HttpPost("start")]
    public async Task<IActionResult> Start([FromBody] StartJourneyRequest request)
    {
        var result = await _mediator.Send(new StartJourneyCommand
        {
            UserId = _currentUser.UserId!.Value,
            StartLatitude = request.StartLatitude,
            StartLongitude = request.StartLongitude,
            EndLatitude = request.EndLatitude,
            EndLongitude = request.EndLongitude
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// End the active journey — asks user if they saw any incidents
    /// </summary>
    [HttpPost("end")]
    public async Task<IActionResult> End()
    {
        var result = await _mediator.Send(new EndJourneyCommand
        {
            UserId = _currentUser.UserId!.Value
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Get your currently active journey
    /// </summary>
    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _mediator.Send(new GetActiveJourneyQuery { UserId = _currentUser.UserId!.Value });
        return Ok(result);
    }

    /// <summary>
    /// Get all your past journeys (paginated)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetMyJourneysQuery
        {
            UserId = _currentUser.UserId!.Value,
            Page = page,
            PageSize = pageSize
        });

        return Ok(result);
    }
}