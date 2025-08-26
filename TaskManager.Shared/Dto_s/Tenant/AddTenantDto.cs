namespace TaskManager.Shared.Dto_s.Tenant;

public class AddTenantDto
{
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string SubscriptionPlan { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
