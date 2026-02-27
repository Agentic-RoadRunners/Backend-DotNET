//Handler is where is job done acttually.
using MediatR;
using SafeRoad.Core.DTOs.Auth;
using SafeRoad.Core.Exceptions;
using SafeRoad.Core.Interfaces.Repositories;
using SafeRoad.Core.Interfaces.Services;
using SafeRoad.Core.Wrappers;

namespace SafeRoad.Core.Features.Auth.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, ApiResponse<AuthResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;// Service injection (Dependency Injection)

    public LoginCommandHandler(IUserRepository userRepository, ITokenService tokenService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
    }

    public async Task<ApiResponse<AuthResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        // Get user by email from the repository
        var user = await _userRepository.GetByEmailAsync(request.Email);
        // Check the constraints of the user.
        if (user == null)
            throw new UnauthorizedException("Email or password is incorrect.");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            throw new UnauthorizedException("Email or password is incorrect.");


        var userWithRoles = await _userRepository.GetWithRolesAsync(user.Id);// Get user with roles to generate access token with role claims
        var roles = userWithRoles?.UserRoles.Select(ur => ur.Role.Name).ToList() ?? new List<string>();// extract role

        var accessToken = _tokenService.GenerateAccessToken(user, roles);// generate jwt

        var response = new AuthResponse
        {
            UserId = user.Id,
            Email = user.Email,
            FullName = user.FullName,
            AccessToken = accessToken,
            Roles = roles
        };//create response DTO

        return ApiResponse<AuthResponse>.Success(response, "Login successful.");// return response
    }
}