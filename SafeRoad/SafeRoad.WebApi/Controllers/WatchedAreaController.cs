using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.DTOs.WatchedArea;
using SafeRoad.Core.Features.WatchedAreas.Commands.CreateWatchedArea;
using SafeRoad.Core.Features.WatchedAreas.Commands.UpdateWatchedArea;
using SafeRoad.Core.Features.WatchedAreas.Commands.DeleteWatchedArea;
using SafeRoad.Core.Features.WatchedAreas.Queries.GetMyWatchedAreas;
using SafeRoad.WebApi.Services;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// Watched areas — manage regions you want to monitor for new incidents
/// </summary>
[ApiController]
[Route("api/watched-areas")]
[Produces("application/json")]
[Authorize]
public class WatchedAreaController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public WatchedAreaController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Get all your watched areas
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetMyWatchedAreasQuery { UserId = _currentUser.UserId!.Value });
        return Ok(result);
    }

    /// <summary>
    /// Add a new watched area (name, center point, radius)
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWatchedAreaRequest request)
    {
        var result = await _mediator.Send(new CreateWatchedAreaCommand
        {
            UserId = _currentUser.UserId!.Value,
            Label = request.Label,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            RadiusInMeters = request.RadiusInMeters
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Update a watched area
    /// </summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateWatchedAreaRequest request)
    {
        var result = await _mediator.Send(new UpdateWatchedAreaCommand
        {
            Id = id,
            UserId = _currentUser.UserId!.Value,
            Label = request.Label,
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            RadiusInMeters = request.RadiusInMeters
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Delete a watched area
    /// </summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteWatchedAreaCommand
        {
            Id = id,
            UserId = _currentUser.UserId!.Value
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }
}