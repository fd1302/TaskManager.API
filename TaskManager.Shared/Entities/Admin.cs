namespace TaskManager.Shared.Entities;

public class Admin
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public string Role { get; set; } = "Admin";
}
