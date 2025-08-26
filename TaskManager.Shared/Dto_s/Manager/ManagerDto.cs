namespace TaskManager.Shared.Dto_s.Manager;

public class ManagerDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
    public string Role { get; set; } = string.Empty;
}
