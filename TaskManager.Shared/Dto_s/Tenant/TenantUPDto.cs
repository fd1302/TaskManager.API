namespace TaskManager.Shared.Dto_s.Tenant;

public class TenantUPDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = string.Empty;
    public string Role { get; set; } = "Role";
}
