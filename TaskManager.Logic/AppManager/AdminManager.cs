using System.Text.RegularExpressions;
using TaskManager.DataAccess.Repository;
using TaskManager.Logic.Mapping;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Admin;

namespace TaskManager.Logic.AppManager;

public class AdminManager
{
    private readonly AdminRepository _adminRepository;
    private readonly TenantRepository _tenantRepository;
    private readonly ManagerRepository _managerRepository;
    private readonly AdminMapping _adminMapping;
    private readonly EmailVerification _emailVerification;
    private readonly PasswordHashing _passHasher;
    public AdminManager(AdminRepository adminRepository, TenantRepository tenantRepository, ManagerRepository managerRepository,
        AdminMapping adminMapping, EmailVerification emailVerification, PasswordHashing passHash)
    {
        _adminRepository = adminRepository ??
            throw new ArgumentNullException(nameof(adminRepository));
        _tenantRepository = tenantRepository ??
            throw new ArgumentNullException(nameof(tenantRepository));
        _managerRepository = managerRepository ??
            throw new ArgumentNullException(nameof(managerRepository));
        _adminMapping = adminMapping ??
            throw new ArgumentNullException(nameof(adminMapping));
        _emailVerification = emailVerification ??
            throw new ArgumentNullException(nameof(emailVerification));
        _passHasher = passHash ??
            throw new ArgumentNullException(nameof(passHash));
    }
    public async Task<AddEntityResultDto> AddAsync(AddAdminDto addAdminDto, Guid tenantId)
    {
        if (addAdminDto.UserName.Length > 100 || string.IsNullOrEmpty(addAdminDto.UserName))
        {
            throw new ArgumentException("Username can't be more than 100 characters or empty.");
        }
        else if (!Regex.IsMatch(addAdminDto.UserName, @"^[a-zA-Z0-9!@#$%^&*]+$"))
        {
            throw new ArgumentException("Use english words(oprtional: numebers, symbols) for your username.");
        }
        else if (addAdminDto.Password.Length > 100 || string.IsNullOrEmpty(addAdminDto.Password))
        {
            throw new ArgumentException("Password can't be more than 100 characters or empty.");
        }
        else if (!Regex.IsMatch(addAdminDto.Password, @"^[a-zA-Z0-9!@#$%^&*]+$"))
        {
            throw new ArgumentException("Use english words, numbers and symbol for your password.");
        }
        var userNameExists = await _adminRepository.AdminUserNameExistsAsync(addAdminDto.UserName);
        if (userNameExists)
        {
            throw new ArgumentException("Username already exists. Try another one.");
        }
        bool validEmail = await _emailVerification.Verify(addAdminDto.Email);
        if (!validEmail)
        {
            throw new ArgumentException("Email is not valid.");
        }
        var admin = _adminMapping.AddAdminDtoTOAdmin(addAdminDto);
        admin.Id = Guid.NewGuid();
        admin.TenantId = tenantId;
        admin.Password = _passHasher.Sha256HashPass(admin.Password);
        DateTime dateTime = DateTime.UtcNow;
        admin.JoinedAt = dateTime;
        bool result = await _adminRepository.AddAsync(admin);
        return new AddEntityResultDto()
        {
            Id = admin.Id,
            IsAdded = true
        };
    }
    public async Task<IEnumerable<AdminDto>?> GetAdminsAsync(string? searchQuery)
    {
        var admins = await _adminRepository.GetAdminsAsync(searchQuery);
        if (admins == null)
        {
            return null;
        }
        var mappedAdmins = admins.Select(_adminMapping.AdminTOAdminDto).ToList();
        return mappedAdmins;
    }
    public async Task<AdminDto?> GetAdminAsync(Guid id)
    {
        var admin = await _adminRepository.GetAdminAsync(id);
        if (admin == null)
        {
            return null;
        }
        var mappedAdmin = _adminMapping.AdminTOAdminDto(admin);
        var tenantName = await _tenantRepository.GetTenantNameAsync(mappedAdmin.TenantId);
        if (tenantName != null)
            mappedAdmin.TenantName = tenantName.Name;
        return mappedAdmin;
    }
    public async Task<IEnumerable<AdminDto>?> GetAdminsWithTenantIdAsync(Guid id)
    {
        var admins = await _adminRepository.GetAdminsWithTenantIdAsync(id);
        if (admins == null)
        {
            return null;
        }
        var mappedAdmins = admins.Select(_adminMapping.AdminTOAdminDto);
        return mappedAdmins;
    }
    public async Task<ResultDto> UpdateAsync(UpdateAdminDto updateAdminDto, Guid id)
    {
        var admin = await _adminRepository.ExistsAsync(id);
        if (!admin)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "Wrong id or admin doesn't exist."
            };
        }
        else if (!string.IsNullOrEmpty(updateAdminDto.UserName))
        {
            if (updateAdminDto.UserName.Length > 100)
            {
                throw new ArgumentException("UserName can't be more than 100 characters.");
            }
        }
        else if (!string.IsNullOrEmpty(updateAdminDto.Email))
        {
            if (updateAdminDto.Email.Length > 256)
            {
                throw new ArgumentNullException("Email address can't be more that 300 characters.");
            }
        }
        var updateAdmin = _adminMapping.UpdateAdminDtoTOAdmin(updateAdminDto);
        bool result = await _adminRepository.UpdateAsync(updateAdmin, id);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "There was an error while handling the request."

            };
        }
        return new ResultDto()
        {
            Success = true,
            Operation = "Update",
            Message = "Update successfuly."
        };
    }
    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        bool adminExists = await _adminRepository.ExistsAsync(id);
        if (!adminExists)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "Admin not found, Wrong id or doesn't exist."
            };
        }
        var result = await _adminRepository.DeleteAsync(id);
        return new ResultDto()
        {
            Success = true,
            Operation = "Delete",
            Message = "Admin deleted successfuly."
        };
    }
    public async Task<ResultDto> PromotionToAdminAsync(PromotionDemotionDto promotionDto)
    {
        if (promotionDto.Role != "Admin")
        {
            throw new Exception("Invalid role.");
        }
        bool adminExists = await _adminRepository.ExistsAsync(promotionDto.UserId);
        var manager = await _managerRepository.GetManagerFullInfoAsync(promotionDto.UserId);
        if (manager == null)
        {
            throw new ArgumentNullException("Manager not found.");
        }
        var admin = _adminMapping.ManagerTOAdmin(manager);
        admin.Role = "Admin";
        var result = await _adminRepository.AddAsync(admin);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Update-Promotion",
                Message = "An error occurred"
            };
        }
        var delete = await _managerRepository.DeleteAsync(manager.Id);
        return new ResultDto()
        {
            Success = true,
            Operation = "Update-Promotion",
            Message = "Promoted"
        };
    }
}
