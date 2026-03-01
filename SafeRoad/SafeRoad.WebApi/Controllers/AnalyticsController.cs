using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.Features.Analytics.Queries.GetAnalyticsOverview;
using SafeRoad.Core.Features.Analytics.Queries.GetCategoryStats;
using SafeRoad.Core.Features.Analytics.Queries.GetTrendData;

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
    [HttpGet("overview")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetOverview()
    {
        var result = await _mediator.Send(new GetAnalyticsOverviewQuery());
        return Ok(result);
    }

    /// <summary>
    /// Get incident count per category
    /// </summary>
    [HttpGet("categories")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCategoryStats()
    {
        var result = await _mediator.Send(new GetCategoryStatsQuery());
        return Ok(result);
    }

    /// <summary>
    /// Get incident trend data over the last N days
    /// </summary>
    [HttpGet("trends")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTrends([FromQuery] int days = 30)
    {
        var result = await _mediator.Send(new GetTrendDataQuery { Days = days });
        return Ok(result);
    }
}
