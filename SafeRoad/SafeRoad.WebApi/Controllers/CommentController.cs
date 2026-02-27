using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.DTOs.Comment;
using SafeRoad.Core.Features.Comments.Commands.CreateComment;
using SafeRoad.Core.Features.Comments.Commands.DeleteComment;
using SafeRoad.Core.Features.Comments.Queries.GetCommentsByIncident;
using SafeRoad.Core.Wrappers;
using SafeRoad.WebApi.Services;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// Comment management — add, list, and delete comments on incidents
/// </summary>
[ApiController]
[Route("api/incidents/{incidentId:guid}/comments")]
[Produces("application/json")]
public class CommentController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public CommentController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Add a comment to an incident
    /// </summary>
    /// <response code="200">Comment created successfully</response>
    /// <response code="400">Validation error</response>
    /// <response code="404">Incident not found</response>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<CommentResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<CommentResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(Guid incidentId, [FromBody] CreateCommentRequest request)
    {
        var result = await _mediator.Send(new CreateCommentCommand
        {
            IncidentId = incidentId,
            UserId = _currentUser.UserId!.Value,
            Content = request.Content
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Get all comments for an incident (paginated)
    /// </summary>
    /// <response code="200">List of comments</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<CommentResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByIncident(Guid incidentId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var result = await _mediator.Send(new GetCommentsByIncidentQuery
        {
            IncidentId = incidentId,
            Page = page,
            PageSize = pageSize
        });

        return Ok(result);
    }

    /// <summary>
    /// Delete your own comment
    /// </summary>
    /// <response code="200">Comment deleted</response>
    /// <response code="400">Cannot delete</response>
    [HttpDelete("{commentId:int}")]
    [Authorize]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid incidentId, int commentId)
    {
        var result = await _mediator.Send(new DeleteCommentCommand
        {
            CommentId = commentId,
            UserId = _currentUser.UserId!.Value
        });

        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }
}