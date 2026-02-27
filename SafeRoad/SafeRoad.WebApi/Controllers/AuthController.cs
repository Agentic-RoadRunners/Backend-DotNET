using MediatR;
using Microsoft.AspNetCore.Mvc;
using SafeRoad.Core.Features.Auth.Commands.Register;
using SafeRoad.Core.Features.Auth.Commands.Login;
using SafeRoad.Core.Features.Auth.Commands.Logout;
using SafeRoad.WebApi.Services;
using Microsoft.AspNetCore.Authorization;
namespace SafeRoad.WebApi.Controllers;

/// <summary>
/// Authentication — register and login
/// </summary>
[ApiController]
[Route("api/auth")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly CurrentUserService _currentUser;

    public AuthController(IMediator mediator, CurrentUserService currentUser)
    {
        _mediator = mediator;
        _currentUser = currentUser;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    /// <param name="command">Registration details (email, password, full name)</param>
    /// <returns>User info and JWT access token</returns>
    /// <response code="200">Registration successful — returns token</response>
    /// <response code="400">Email already exists or validation error</response>
    [HttpPost("register")]
    [ProducesResponseType(typeof(SafeRoad.Core.Wrappers.ApiResponse<SafeRoad.Core.DTOs.Auth.AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SafeRoad.Core.Wrappers.ApiResponse<SafeRoad.Core.DTOs.Auth.AuthResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await _mediator.Send(command);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }

    /// <summary>
    /// Login with email and password
    /// </summary>
    /// <param name="command">Login credentials (email, password)</param>
    /// <returns>User info and JWT access token</returns>
    /// <response code="200">Login successful — returns token</response>
    /// <response code="400">Invalid email or password</response>
    [HttpPost("login")]
    [ProducesResponseType(typeof(SafeRoad.Core.Wrappers.ApiResponse<SafeRoad.Core.DTOs.Auth.AuthResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(SafeRoad.Core.Wrappers.ApiResponse<SafeRoad.Core.DTOs.Auth.AuthResponse>), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Login(LoginCommand command)// LoginCommand does the all job it is just a representation of endpoint
    {
        var result = await _mediator.Send(command);
        if (!result.Succeeded) return BadRequest(result);
        return Ok(result);
    }


    /// <summary>
    /// Logout — removes device tokens and invalidates session
    /// </summary>
    /// <remarks>
    /// Client must discard the JWT token after calling this endpoint.
    /// Since JWT is stateless, the token remains technically valid until expiration,
    /// but device tokens are removed to stop push notifications.
    /// </remarks>
    /// <response code="200">Logout successful</response>
    /// <response code="401">Not authenticated</response>
    [HttpPost("logout")]
    [Authorize]
    [ProducesResponseType(typeof(SafeRoad.Core.Wrappers.ApiResponse<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Logout()
    {
        var result = await _mediator.Send(new LogoutCommand
        {
            UserId = _currentUser.UserId!.Value
        });

        return Ok(result);
    }
}