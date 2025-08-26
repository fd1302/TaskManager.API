using System.Text.RegularExpressions;
using TaskManager.DataAccess.Repository;
using TaskManager.Logic.Mapping;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Subscription;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.AppManager;

public class SubPlanManager
{
    private readonly SubscriptionRepository _subscriptionRepository;
    private readonly SubscriptionMapping _subscriptionMapping;
    private readonly TenantRepository _tenantRepository;
    public SubPlanManager(SubscriptionRepository subscriptionRepository, SubscriptionMapping subscriptionMapping, TenantRepository tenantRepository)
    {
        _subscriptionRepository = subscriptionRepository ??
            throw new ArgumentNullException(nameof(subscriptionRepository));
        _subscriptionMapping = subscriptionMapping ??
            throw new ArgumentNullException(nameof(subscriptionMapping));
        _tenantRepository = tenantRepository ??
            throw new ArgumentNullException(nameof(tenantRepository));
    }
    public async Task<AddEntityResultDto> AddAsync(AddSubscriptionDto addSubscriptionDto)
    {
        bool tenantExists = await _tenantRepository.ExistsAsync(null, addSubscriptionDto.TenantId);
        if (!tenantExists)
        {
            throw new ArgumentException("Tenant not found. Check id.");
        }
        else if (addSubscriptionDto.Duration < 1)
        {
            throw new ArgumentException("Duration can't be less than 1.");
        }
        var tenantName = await _tenantRepository.GetTenantNameAsync(addSubscriptionDto.TenantId);
        var subPlan = new Subscription();
        subPlan.Id = Guid.NewGuid();
        subPlan.TenantId = addSubscriptionDto.TenantId;
        subPlan.TenantName = tenantName!.Name;
        subPlan.StartDate = DateTime.Now;
        subPlan.EndDate = DateTime.Now.AddMonths(addSubscriptionDto.Duration);
        subPlan.Duration = $"{addSubscriptionDto.Duration} Months";
        var result = await _subscriptionRepository.AddAsync(subPlan);
        if (!result)
        {
            throw new ArgumentException("There was an error while handling the request.");
        }
        var subscriptionHistory = _subscriptionMapping.SubscriptionToAddSubscriptionHistoryDto(subPlan);
        await AddSubscriptionHistoryAsync(subscriptionHistory);
        return new AddEntityResultDto()
        {
            Id = subPlan.Id,
            IsAdded = true
        };
    }
    public async Task<bool> AddSubscriptionHistoryAsync(AddSubscriptionHistoryDto addSubscriptionHistoryDto)
    {
        var sh = _subscriptionMapping.AddSHToSubscriptionHistory(addSubscriptionHistoryDto);
        sh.Id = Guid.NewGuid();
        var result = await _subscriptionRepository.AddSubscriptionHistoryAsync(sh);
        return result;
    }
    public async Task<IEnumerable<SubscriptionDto>?> GetSubscriptionsAsync()
    {
        var subscriptions = await _subscriptionRepository.GetSubscriptionsAsync();
        if (subscriptions == null)
        {
            return null;
        }
        var mappedSubPlans = subscriptions.Select(_subscriptionMapping.SubscriptionToSubscriptionDto);
        return mappedSubPlans;
    }
    public async Task<SubscriptionDto?> GetSubscriptionAsync(Guid? id, Guid? tenantId)
    {
        var subPlan = await _subscriptionRepository.GetSubscriptionAsync(id, tenantId);
        if (subPlan == null)
        {
            return null;
        }
        var mappedSubPlan = _subscriptionMapping.SubscriptionToSubscriptionDto(subPlan);
        return mappedSubPlan;
    }
    public async Task<ResultDto> UpdateSubscriptinoAsync(Guid id, UpdateSubscriptionDto updateSubscriptionDto)
    {
        var subscription = await _subscriptionRepository.GetSubscriptionAsync(null, id);
        if (subscription == null)
        {
            throw new ArgumentNullException("Subscription plan not found.");
        }
        else if (DateTime.Now < subscription.EndDate)
        {
            string duration = Regex.Match(subscription.Duration, @"\d+").Value;
            int durationVal = int.Parse(duration);
            subscription.Duration = $"{durationVal + updateSubscriptionDto.Duration} Months";
            subscription.EndDate = subscription.EndDate.AddMonths(updateSubscriptionDto.Duration);
            var result = await _subscriptionRepository.UpdateSubscriptionAsync(id, subscription);
            if (result)
            {
                return new ResultDto()
                {
                    Operation = "Update",
                    Message = "Updated successfuly.",
                    Success = true
                };
            }
        }
        else if (DateTime.Now > subscription.EndDate)
        {
            subscription.StartDate = DateTime.Now;
            subscription.EndDate = DateTime.Now.AddMonths(updateSubscriptionDto.Duration);
            subscription.Duration = $"{updateSubscriptionDto.Duration} Months";
            var updateResult = await _subscriptionRepository.UpdateSubscriptionAsync(id, subscription);
            if (updateResult)
            {
                return new ResultDto()
                {
                    Operation = "Update",
                    Message = "Updated successfuly",
                    Success = true
                };
            }
        }
        return new ResultDto()
        {
            Operation = "Update",
            Message = "Update failed."
        };
    }
    public async Task<IEnumerable<SubscriptionHistoriesDto>?> GetSubscriptionHistoriesAsync(string? searchQuery)
    {
        var sh = await _subscriptionRepository.GetSubscriptionHistoriesAsync(searchQuery);
        if (sh == null)
        {
            return null;
        }
        var mappedSH = sh.Select(_subscriptionMapping.SHToSHDto);
        return mappedSH;
    }
    public async Task<IEnumerable<SubscriptionHistoriesDto>?> GetSubscriptionHistoriesWithTenantIdAsync(Guid id)
    {
        var sh = await _subscriptionRepository.GetSubscriptionHistoriesWithTenantIdAsync(id);
        if (sh == null)
        {
            return null;
        }
        var mappedSH = sh.Select(_subscriptionMapping.SHToSHDto);
        return mappedSH;
    }
}
