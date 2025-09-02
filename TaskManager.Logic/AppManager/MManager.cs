using TaskManager.DataAccess.Repository;
using TaskManager.Logic.Mapping;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Authentication;
using TaskManager.Shared.Dto_s.Manager;

namespace TaskManager.Logic.AppManager;

public class MManager
{
    private readonly ManagerRepository _managerRepository;
    private readonly MemberRepository _memberRepository;
    private readonly AdminRepository _adminRepository;
    private readonly ManagerMapping _managerMapping;
    private readonly EmailVerification _emailVerification;
    private readonly PasswordHashing _passHasher;
    private readonly AuthService _authService;
    public MManager(ManagerRepository managerRepository, MemberRepository memberRepository, AdminRepository adminRepository,
        ManagerMapping managerMapping, EmailVerification emailVerification, PasswordHashing passHash, AuthService authService)
    {
        _managerRepository = managerRepository ??
            throw new ArgumentNullException(nameof(managerRepository));
        _memberRepository = memberRepository ??
            throw new ArgumentNullException(nameof(memberRepository));
        _adminRepository = adminRepository ??
            throw new ArgumentNullException(nameof(adminRepository));
        _managerMapping = managerMapping ??
            throw new ArgumentNullException(nameof(managerMapping));
        _emailVerification = emailVerification ??
            throw new ArgumentNullException(nameof(emailVerification));
        _passHasher = passHash ??
            throw new ArgumentNullException(nameof(passHash));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    public async Task<AddEntityResultDto> AddAsync(AddManagerDto addManagerDto)
    {
        if (addManagerDto.UserName.Length > 100)
        {
            throw new ArgumentException("Username can't be more than 100 characters.");
        }
        else if (string.IsNullOrEmpty(addManagerDto.UserName) || addManagerDto.UserName.Contains(' '))
        {
            throw new ArgumentException("Username can't be empty or contain white space.");
        }
        else if (addManagerDto.Email.Length < 10 || addManagerDto.Email.Length > 256)
        {
            throw new ArgumentException("Email must be between 20-256 characters.");
        }
        else if (string.IsNullOrEmpty(addManagerDto.Email) || addManagerDto.Email.Contains(' '))
        {
            throw new ArgumentException("Email can't be empty or contant white space.");
        }
        else if (addManagerDto.Password.Length < 5 || addManagerDto.Password.Length > 100)
        {
            throw new ArgumentException("Password must be between 10-100 characters.");
        }
        else if (string.IsNullOrEmpty(addManagerDto.Password) || addManagerDto.Password.Contains(' '))
        {
            throw new ArgumentException("Password can't be empty or contan white space.");
        }
        bool userNameExists = await _managerRepository.ExistsAsync(addManagerDto.UserName, null);
        if (userNameExists)
        {
            throw new ArgumentException("Username already exists.");
        }
        bool checkEmail = await _emailVerification.Verify(addManagerDto.Email);
        if (!checkEmail)
        {
            throw new ArgumentException("Email is not valid.");
        }
        var manager = _managerMapping.AddManagerDtoTOManager(addManagerDto);
        manager.Id = Guid.NewGuid();
        manager.Password = _passHasher.Sha256HashPass(manager.Password);
        manager.JoinedAt = DateTime.UtcNow;
        bool result = await _managerRepository.AddAsync(manager);
        if (!result)
        {
            return new AddEntityResultDto();
        }
        var auth = new AuthenticationDto()
        {
            UserName = manager.UserName,
            Password = addManagerDto.Password
        };
        var token = await _authService.ManagerAuthenticationAsync(auth);
        return new AddEntityResultDto()
        {
            Id = manager.Id,
            IsAdded = true,
            Token = token.Token
        };
    }
    public async Task<IEnumerable<ManagerDto>?> GetManagersAsync(string? searchQuery)
    {
        var managers = await _managerRepository.GetManagersAsync(searchQuery);
        if (managers == null)
        {
            return null;
        }
        var mappedManagers = managers.Select(_managerMapping.ManagerTOManagerDto);
        return mappedManagers;
    }
    public async Task<ManagerDto?> GetManagerAsync(Guid id)
    {
        var manager = await _managerRepository.GetManagerAsync(id);
        if (manager == null)
        {
            return null;
        }
        var mappedManager = _managerMapping.ManagerTOManagerDto(manager);
        return mappedManager;
    }
    public async Task<IEnumerable<ManagerDto>?> GetManagersWithTenantIdAsync(UserTokenInfoDto userInfo)
    {
        var id = userInfo.Id;
        if (userInfo.Role != "Tenant" && userInfo.TenantId != null)
        {
            id = (Guid)userInfo.TenantId;
        }
        var managers = await _managerRepository.GetManagersWithTenantIdAsync(id);
        if (managers == null)
        {
            return null;
        }
        var mappedManagers = managers.Select(_managerMapping.ManagerTOManagerDto);
        return mappedManagers;
    }
    public async Task<ResultDto> UpdateAsync(UpdateManagerDto updateManagerDto, Guid id)
    {
        bool managerExists = await _managerRepository.ExistsAsync(null, id);
        if (!managerExists)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "Wrong id or manager doesn't exist."
            };
        }
        else if (!string.IsNullOrEmpty(updateManagerDto.UserName))
        {
            if (updateManagerDto.UserName.Length < 5 || updateManagerDto.UserName.Length > 100)
            {
                throw new ArgumentException("Username must be between 5-100 characters.");
            }
        }
        else if (!string.IsNullOrEmpty(updateManagerDto.Email))
        {
            if (updateManagerDto.Email.Length < 20 || updateManagerDto.Email.Length > 100)
            {
                throw new ArgumentException("Email must be between 20-256 characters.");
            }
        }
        var manager = _managerMapping.UpdateManagerDtoTOManager(updateManagerDto);
        bool result = await _managerRepository.UpdateAsync(manager, id);
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
            Message = "Updated successfuly."
        };
    }
    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        bool managerExists = await _managerRepository.ExistsAsync(null, id);
        if (!managerExists)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "Manager not found, Wrong id or doesn't exist."
            };
        }
        var result = await _managerRepository.DeleteAsync(id);
        return new ResultDto()
        {
            Success = true,
            Operation = "Delete",
            Message = "Manager deleted successfuly."
        };
    }
    public async Task<ResultDto> PromotionToManagerAsync(PromotionDemotionDto promotionDto, Guid tenantId)
    {
        if (promotionDto.Role != "Manager")
        {
            throw new Exception("Invalid role.");
        }
        var member = await _memberRepository.GetMemberForPromotionAsync(promotionDto.UserId);
        if (member == null)
        {
            throw new ArgumentNullException("Member not found.");
        }
        var manager = _managerMapping.MemberTOManager(member);
        manager.TenantId = tenantId;
        manager.Role = "Manager";
        bool result = await _managerRepository.AddAsync(manager);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Update(Promotion)",
                Message = "An error occurred"
            };
        }
        var delete = await _memberRepository.DeleteAsync(member.Id);
        return new ResultDto()
        {
            Success = true,
            Operation = "Update-Promotion",
            Message = "Promoted successfuly"
        };
    }
    public async Task<ResultDto> DemotionToManagerAsync(PromotionDemotionDto demotionDto, Guid tenantId)
    {
        if (demotionDto.Role != "Manager")
        {
            throw new Exception("Invalid role.");
        }
        var admin = await _adminRepository.GetFullAdminInfoAsync(null, demotionDto.UserId);
        if (admin == null)
        {
            throw new ArgumentNullException("Admin not found.");
        }
        var manager = _managerMapping.AdminTOManager(admin);
        manager.Role = "Manager";
        bool result = await _managerRepository.AddAsync(manager);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Update(Demotion)",
                Message = "An error accurred"
            };
        }
        var delete = await _adminRepository.DeleteAsync(admin.Id);
        return new ResultDto()
        {
            Success = true,
            Operation = "Update(Demotion)",
            Message = "Demoted successfuly"
        };
    }
}
