using TaskManager.Shared.Dto_s.Manager;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.Mapping;

public class ManagerMapping
{
    public Manager AddManagerDtoTOManager(AddManagerDto addManagerDto)
    {
        return new Manager()
        {
            TenantId = addManagerDto.TenantId,
            UserName = addManagerDto.UserName,
            Email = addManagerDto.Email,
            Password = addManagerDto.Password
        };
    }
    public ManagerDto ManagerTOManagerDto(Manager manager)
    {
        return new ManagerDto()
        {
            Id = manager.Id,
            TenantId = manager.TenantId,
            UserName = manager.UserName,
            Email = manager.Email,
            JoinedAt = manager.JoinedAt,
            Role = manager.Role
        };
    }
    public Manager UpdateManagerDtoTOManager(UpdateManagerDto updateManagerDto)
    {
        return new Manager()
        {
            UserName = updateManagerDto.UserName,
            Email = updateManagerDto.Email
        };
    }
    public Manager MemberTOManager(Member member)
    {
        return new Manager()
        {
            Id = member.Id,
            UserName = member.UserName,
            Password = member.Password,
            Email = member.Email,
            JoinedAt = member.JoinedAt
        };
    }
    public Manager AdminTOManager(Admin admin)
    {
        return new Manager()
        {
            Id = admin.Id,
            TenantId = admin.TenantId,
            UserName = admin.UserName,
            Password = admin.Password,
            Email = admin.Email,
            JoinedAt = admin.JoinedAt
        };
    }
}
