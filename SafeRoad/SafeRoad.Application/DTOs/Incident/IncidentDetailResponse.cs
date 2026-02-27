
namespace SafeRoad.Core.DTOs.Incident;

public class IncidentDetailResponse : IncidentResponse
{
    public List<CommentDto> Comments { get; set; } = new();
    public List<VerificationDto> Verifications { get; set; } = new();
}

public class CommentDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public string Content { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}

public class VerificationDto
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
    public bool IsPositive { get; set; }
    public DateTime CreatedAt { get; set; }
}