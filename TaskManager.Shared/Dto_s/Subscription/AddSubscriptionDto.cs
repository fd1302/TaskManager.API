namespace TaskManager.Shared.Dto_s.Subscription;

public class AddSubscriptionDto
{
    public Guid TenantId { get; set; }
    public int Duration { get; set; }
}
