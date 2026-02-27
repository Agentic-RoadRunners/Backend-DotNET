using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.DTOs.IncidentCategory;
using SafeRoad.Core.Features.IncidentCategories.Queries.GetAllIncidentCategories;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// Incident categories — list all available incident types
/// </summary>
[ApiController]
[Route("api/incident-categories")]
[Produces("application/json")]
public class IncidentCategoryController : ControllerBase
{
    private readonly IMediator _mediator;

    public IncidentCategoryController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get all incident categories
    /// </summary>
    /// <response code="200">List of incident categories</response>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<List<IncidentCategoryResponse>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAllIncidentCategoriesQuery());
        return Ok(result);
    }
}