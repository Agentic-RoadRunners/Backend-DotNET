using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.Features.Analytics.Queries.GetAnalyticsOverview;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// Platform-wide analytics and statistics
/// </summary>
[ApiController]
[Route("api/analytics")]
[Produces("application/json")]
public class AnalyticsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AnalyticsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Get platform-wide overview statistics
    /// </summary>
    /// <returns>Total incidents, total users, resolved count, municipality count</returns>
    /// <response code="200">Returns the analytics overview</response>
    [HttpGet("overview")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverview()
    {
        var result = await _mediator.Send(new GetAnalyticsOverviewQuery());
        return Ok(result);
    }
}
