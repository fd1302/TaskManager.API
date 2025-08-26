namespace TaskManager.Shared.Dto_s.Subscription;

public class AddSubscriptionHistoryDto
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string TenantName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Duration { get; set; } = string.Empty;
}
