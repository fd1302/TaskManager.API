using TaskManager.Shared.Dto_s.Admin;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.Mapping;

public class AdminMapping
{
    public Admin AddAdminDtoTOAdmin(AddAdminDto addAdminDto)
    {
        return new Admin()
        {
            UserName = addAdminDto.UserName,
            Password = addAdminDto.Password,
            Email = addAdminDto.Email
        };
    }
    public AdminDto AdminTOAdminDto(Admin admin)
    {
        return new AdminDto()
        {
            Id = admin.Id,
            TenantId = admin.TenantId,
            UserName = admin.UserName,
            Email = admin.Email,
            JoinedAt = admin.JoinedAt,
            Role = admin.Role
        };
    }
    public Admin UpdateAdminDtoTOAdmin(UpdateAdminDto updateAdminDto)
    {
        return new Admin()
        {
            UserName = updateAdminDto.UserName,
            Email = updateAdminDto.Email
        };
    }
    public Admin ManagerTOAdmin(Manager manager)
    {
        return new Admin()
        {
            Id = manager.Id,
            TenantId = manager.TenantId,
            UserName = manager.UserName,
            Password = manager.Password,
            Email = manager.Email,
            JoinedAt = manager.JoinedAt
        };
    }
}
