namespace SafeRoad.Core.DTOs.User;

public class AdminUserResponse
{
    public Guid Id { get; set; }
    public string Email { get; set; } = null!;
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public int TrustScore { get; set; }
    public string Status { get; set; } = null!;
    public List<string> Roles { get; set; } = new();
    public int TotalIncidents { get; set; }
    public int TotalComments { get; set; }
    public DateTime CreatedAt { get; set; }
}
