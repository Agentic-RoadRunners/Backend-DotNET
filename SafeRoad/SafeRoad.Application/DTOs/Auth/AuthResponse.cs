
namespace SafeRoad.Core.DTOs.Auth;

public class AuthResponse
{
    public Guid UserId { get; set; }
    public string Email { get; set; } = null!;
    public string? FullName { get; set; }
    public string AccessToken { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
}