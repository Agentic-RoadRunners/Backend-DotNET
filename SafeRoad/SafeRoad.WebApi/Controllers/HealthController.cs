using Microsoft.AspNetCore.Mvc;

namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// System health checks
/// </summary>
[ApiController]
[Route("api/health")]
[Produces("application/json")]
public class HealthController : ControllerBase
{
    /// <summary>
    /// Check if the API is running
    /// </summary>
    /// <returns>Health status and timestamp</returns>
    /// <response code="200">API is healthy</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult Get()
    {
        return Ok(new { status = "Healthy", timestamp = DateTime.UtcNow });
    }
}