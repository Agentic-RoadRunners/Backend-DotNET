using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.DTOs.Incident;
using SafeRoad.Core.DTOs.User;
using SafeRoad.Core.Features.Incidents.Commands.DeleteIncidentByAdmin;
using SafeRoad.Core.Features.Incidents.Commands.UpdateIncidentByAdmin;
using SafeRoad.Core.Features.Incidents.Queries.GetAllIncidents;
using SafeRoad.Core.Features.Users.Commands.BanUser;
using SafeRoad.Core.Features.Users.Commands.UnbanUser;
using SafeRoad.Core.Features.Users.Commands.UpdateUserByAdmin;
using SafeRoad.Core.Features.Users.Queries.GetAllUsers;

namespace SafeRoad.WebApi.Controllers;

[ApiController]
[Route("api/admin")]
[Produces("application/json")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly IMediator _mediator;

    public AdminController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all users with pagination and filtering (Admin only)
    /// </summary>
    [HttpGet("users")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllUsers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? search = null,
        [FromQuery] string? role = null,
        [FromQuery] string? status = null)
    {
        var result = await _mediator.Send(new GetAllUsersQuery
        {
            Page = page,
            PageSize = pageSize,
            Search = search,
            Role = role,
            Status = status
        });
        return Ok(result);
    }

    /// <summary>
    /// Ban a user (Admin only)
    /// </summary>
    [HttpPatch("users/{id:guid}/ban")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> BanUser(Guid id)
    {
        var result = await _mediator.Send(new BanUserCommand { UserId = id });
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Unban a user (Admin only)
    /// </summary>
    [HttpPatch("users/{id:guid}/unban")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UnbanUser(Guid id)
    {
        var result = await _mediator.Send(new UnbanUserCommand { UserId = id });
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Update a user (Admin only)
    /// </summary>
    [HttpPut("users/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UpdateUserByAdminRequest request)
    {
        var result = await _mediator.Send(new UpdateUserByAdminCommand
        {
            UserId = id,
            FullName = request.FullName,
            Roles = request.Roles
        });
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Update an incident (Admin only)
    /// </summary>
    [HttpPut("incidents/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateIncident(Guid id, [FromBody] AdminIncidentUpdateRequest request)
    {
        var result = await _mediator.Send(new UpdateIncidentByAdminCommand
        {
            IncidentId = id,
            Title = request.Title,
            Description = request.Description,
            CategoryId = request.CategoryId,
            Status = request.Status
        });
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Delete an incident (Admin only)
    /// </summary>
    [HttpDelete("incidents/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> DeleteIncident(Guid id)
    {
        var result = await _mediator.Send(new DeleteIncidentByAdminCommand { IncidentId = id });
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Get all incidents for admin management (Admin only)
    /// </summary>
    [HttpGet("incidents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllIncidents([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetAllIncidentsQuery { Page = page, PageSize = pageSize });
        return Ok(result);
    }
}
