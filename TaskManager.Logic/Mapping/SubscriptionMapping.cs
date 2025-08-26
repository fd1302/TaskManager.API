using TaskManager.Shared.Dto_s.Subscription;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.Mapping;

public class SubscriptionMapping
{
    public SubscriptionDto SubscriptionToSubscriptionDto(Subscription subscription)
    {
        return new SubscriptionDto()
        {
            Id = subscription.Id,
            TenantId = subscription.TenantId,
            TenantName = subscription.TenantName,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Duration = subscription.Duration
        };
    }
    public AddSubscriptionHistoryDto SubscriptionToAddSubscriptionHistoryDto(Subscription subscription)
    {
        return new AddSubscriptionHistoryDto()
        {
            Id = subscription.Id,
            TenantId = subscription.TenantId,
            TenantName = subscription.TenantName,
            StartDate = subscription.StartDate,
            EndDate = subscription.EndDate,
            Duration = subscription.Duration
        };
    }
    public SubscriptionHistory AddSHToSubscriptionHistory(AddSubscriptionHistoryDto addSubscriptionHistoryDto)
    {
        return new SubscriptionHistory()
        {
            TenantId = addSubscriptionHistoryDto.TenantId,
            TenantName = addSubscriptionHistoryDto.TenantName,
            StartDate = addSubscriptionHistoryDto.StartDate,
            EndDate = addSubscriptionHistoryDto.EndDate,
            Duration = addSubscriptionHistoryDto.Duration
        };
    }
    public SubscriptionHistoriesDto SHToSHDto(SubscriptionHistory subscriptionHistory)
    {
        return new SubscriptionHistoriesDto()
        {
            Id = subscriptionHistory.Id,
            TenantId = subscriptionHistory.TenantId,
            TenantName = subscriptionHistory.TenantName,
            StartDate = subscriptionHistory.StartDate,
            EndDate = subscriptionHistory.EndDate,
            Duration = subscriptionHistory.Duration
        };
    }
}
