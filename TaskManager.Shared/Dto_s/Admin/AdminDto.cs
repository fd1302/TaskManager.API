namespace TaskManager.Shared.Dto_s.Admin;

public class AdminDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string? TenantName { get; set; }
    public required string UserName { get; set; }
    public required string Email { get; set; }
    public DateTime JoinedAt { get; set; }
    public string Role { get; set; } = "Admin";
}
