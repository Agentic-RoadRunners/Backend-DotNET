namespace SafeRoad.Core.DTOs.User;

public class UpdateUserByAdminRequest
{
    public string? FullName { get; set; }
    public List<string>? Roles { get; set; }
}
