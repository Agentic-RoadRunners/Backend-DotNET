
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.Features.Incidents.Commands.CreateIncident;
using SafeRoad.Core.Features.Incidents.Commands.DeleteIncident;
using SafeRoad.Core.Features.Incidents.Commands.UpdateIncidentStatus;
using SafeRoad.Core.Features.Incidents.Queries.GetAllIncidents;
using SafeRoad.Core.Features.Incidents.Queries.GetIncidentById;
using SafeRoad.Core.Features.Incidents.Queries.GetMyIncidents;
using SafeRoad.Core.Features.Incidents.Queries.GetNearbyIncidents;
using SafeRoad.Core.Features.Incidents.Queries.GetByMunicipality;
using SafeRoad.WebApi.Services;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// Incident management — create, query, update, and delete traffic incidents
/// </summary>
[ApiController]
[Route("api/incidents")]
[Produces("application/json")]
public class IncidentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public IncidentController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Report a new traffic incident
    /// </summary>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateIncidentRequest request)
    {
        var command = new CreateIncidentCommand
        {
            UserId = _currentUser.UserId!.Value,
            CategoryId = request.CategoryId,
            MunicipalityId = request.MunicipalityId,
            Title = request.Title,
            Description = request.Description,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        var result = await _mediator.Send(command);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Get all incidents (paginated)
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetAllIncidentsQuery
        {
            Page = page,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Get nearby incidents based on location
    /// </summary>
    [HttpGet("nearby")]
    public async Task<IActionResult> GetNearby(
        [FromQuery] double latitude,
        [FromQuery] double longitude,
        [FromQuery] int radiusMeters = 5000,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetNearbyIncidentsQuery
        {
            Latitude = latitude,
            Longitude = longitude,
            RadiusMeters = radiusMeters,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Get incident details by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _mediator.Send(new GetIncidentByIdQuery { Id = id });
        if (!result.Succeeded) return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Get incidents reported by the authenticated user
    /// </summary>
    [HttpGet("my")]
    [Authorize]
    public async Task<IActionResult> GetMy([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetMyIncidentsQuery
        {
            UserId = _currentUser.UserId!.Value,
            Page = page,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Get incidents by municipality
    /// </summary>
    [HttpGet("by-municipality/{municipalityId:int}")]
    [Authorize(Roles = "Admin,Municipality")]
    public async Task<IActionResult> GetByMunicipality(int municipalityId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetByMunicipalityQuery
        {
            MunicipalityId = municipalityId,
            Page = page,
            PageSize = pageSize
        });
        return Ok(result);
    }

    /// <summary>
    /// Update incident status (Admin/Municipality only)
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    [Authorize(Roles = "Admin,Municipality")]
    public async Task<IActionResult> UpdateStatus(Guid id, [FromBody] UpdateIncidentStatusCommand command)
    {
        command.IncidentId = id;
        var result = await _mediator.Send(command);
        if (!result.Succeeded) return NotFound(result);
        return Ok(result);
    }

    /// <summary>
    /// Delete an incident (owner only)
    /// </summary>
    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteIncidentCommand
        {
            IncidentId = id,
            UserId = _currentUser.UserId!.Value
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }
}