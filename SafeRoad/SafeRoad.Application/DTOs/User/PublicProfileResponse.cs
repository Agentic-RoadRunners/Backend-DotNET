
namespace SafeRoad.Core.DTOs.User;

public class PublicProfileResponse
{
    public Guid Id { get; set; }
    public string? FullName { get; set; }
    public string? AvatarUrl { get; set; }
    public int TrustScore { get; set; }
    public DateTime CreatedAt { get; set; }
}