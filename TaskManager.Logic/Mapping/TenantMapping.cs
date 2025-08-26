using TaskManager.Shared.Dto_s.Tenant;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.Mapping;

public class TenantMapping
{
    public Tenant AddTenantDtoToTenant(AddTenantDto addTenantDto)
    {
        return new Tenant()
        {
            UserName = addTenantDto.UserName,
            Password = addTenantDto.Password,
            Name = addTenantDto.Name,
            SubscriptionPlan = addTenantDto.SubscriptionPlan,
            Description = addTenantDto.Description,
        };
    }
    public TenantDto TenantToTenantDto(Tenant tenant)
    {
        return new TenantDto()
        {
            Id = tenant.Id,
            UserName = tenant.UserName,
            Name = tenant.Name,
            SubscriptionPlan = tenant.SubscriptionPlan,
            Description = tenant.Description,
            CreatedAt = tenant.CreatedAt,
            IsActive = tenant.IsActive
        };
    }
    public Tenant UpdateTenantDtoTOTenant(UpdateTenantDto updateTenantDto)
    {
        return new Tenant()
        {
            Name = updateTenantDto.Name,
            Description = updateTenantDto.Description
        };
    }
}
