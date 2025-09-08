using System.Text.RegularExpressions;
using TaskManager.DataAccess.Repository;
using TaskManager.Logic.Mapping;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Authentication;
using TaskManager.Shared.Dto_s.Tenant;

namespace TaskManager.Logic.AppManager;

public class TenantManager
{
    private readonly TenantRepository _tenantRepository;
    private readonly TenantMapping _tenantMapping;
    private readonly PasswordHashing _passHasher;
    private readonly AuthService _authService;
    public TenantManager(TenantRepository tenantRepository, TenantMapping tenantMapping, PasswordHashing passHasher, AuthService authService)
    {
        _tenantRepository = tenantRepository ??
            throw new ArgumentNullException(nameof(tenantRepository));
        _tenantMapping = tenantMapping ??
            throw new ArgumentNullException(nameof(tenantMapping));
        _passHasher = passHasher ??
            throw new ArgumentNullException(nameof(passHasher));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    public async Task<AddEntityResultDto> AddAsync(AddTenantDto addTenantDto)
    {
        if (addTenantDto.UserName.Length > 100 || string.IsNullOrEmpty(addTenantDto.UserName))
        {
            throw new ArgumentException("Username can't be more than 100 characters or empty.");
        }
        else if (!Regex.IsMatch(addTenantDto.UserName, @"^[a-zA-Z0-9!@#$%^&*]+$"))
        {
            throw new ArgumentException("Use english words(oprtional: numebers, symbols) for your username.");
        }
        else if (!Regex.IsMatch(addTenantDto.Password, @"^[a-zA-Z0-9!@#$%^&*]+$"))
        {
            throw new ArgumentException("Use english words, numbers and symbol for your password.");
        }
        else if (addTenantDto.Name.Length > 100 || string.IsNullOrEmpty(addTenantDto.Name))
        {
            throw new ArgumentException("Name can't be more than 100 characters or empty.");
        }
        else if (addTenantDto.Description.Length > 300 || string.IsNullOrEmpty(addTenantDto.Description))
        {
            throw new ArgumentException("Description can't be more that 300 characters or empty.");
        }
        bool checkUserName = await _tenantRepository.ExistsAsync(addTenantDto.UserName, null);
        if (checkUserName)
        {
            throw new ArgumentException("Username already exists. Try another one.");
        }
        var tenant = _tenantMapping.AddTenantDtoToTenant(addTenantDto);
        tenant.Id = Guid.NewGuid();
        DateTime dateTime = DateTime.UtcNow;
        tenant.Password = _passHasher.Sha256HashPass(tenant.Password);
        //(Remove MS) DateTime trimmed = dateTime.AddTicks(-(dateTime.Ticks % TimeSpan.TicksPerSecond));
        tenant.CreatedAt = dateTime;
        int result = await _tenantRepository.AddAsync(tenant);
        if (result == 0)
        {
            return new AddEntityResultDto()
            {
                Id = null,
                IsAdded = false
            };
        }
        var loginDto = new AuthenticationDto()
        {
            UserName = addTenantDto.UserName,
            Password = addTenantDto.Password
        };
        var tenantJwt = await _authService.TenantAuthenticationAsync(loginDto);
        if (!tenantJwt.IsSuccessful)
        {
            throw new Exception(tenantJwt.ErrorMessage);
        }
        return new AddEntityResultDto()
        {
            Id = tenant.Id,
            IsAdded = true,
            Token = tenantJwt.Token
        };
    }
    public async Task<IEnumerable<TenantDto>?> GetTenantsAsync(string? searchQuery)
    {
        var tenants = await _tenantRepository.GetTenantsAsync(searchQuery);
        if (tenants == null)
        {
            return null;
        }
        var result = tenants.Select(_tenantMapping.TenantToTenantDto);
        return result;
    }
    public async Task<TenantDto?> GetTenantAsync(Guid id)
    {
        var tenant = await _tenantRepository.GetTenantAsync(id);
        if (tenant == null)
        {
            return null;
        }
        return _tenantMapping.TenantToTenantDto(tenant);
    }
    public async Task<ResultDto> UpdateAsync(UpdateTenantDto updateDto, Guid id)
    {
        var tenantExist = await _tenantRepository.ExistsAsync(string.Empty, id);
        if (!tenantExist)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "User not found, Wrong id or doesn't exist"
            };
        }
        if (!string.IsNullOrEmpty(updateDto.Name))
        {
            if (updateDto.Name.Length > 100)
            {
                throw new ArgumentException("Name can't be more than 100 characters");
            }
        }
        if (!string.IsNullOrEmpty(updateDto.Description))
        {
            if (updateDto.Description.Length > 300)
            {
                throw new ArgumentNullException("Description can't be more that 300 characters");
            }
        }
        var tenant = _tenantMapping.UpdateTenantDtoTOTenant(updateDto);
        bool result = await _tenantRepository.UpdateAsync(tenant, id);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "There was an error while handling the request"

            };
        }
        return new ResultDto()
        {
            Success = true,
            Operation = "Update",
            Message = "Updated successfuly"
        };
    }
    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var checkTenant = await _tenantRepository.GetTenantAsync(id);
        if (checkTenant == null)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "Tenant not found, Wrong id or doesn't exist"
            };
        }
        var result = await _tenantRepository.DeleteAsync(id);
        return new ResultDto()
        {
            Success = true,
            Operation = "Delete",
            Message = "Tenant deleted successfuly"
        };
    }
    public async Task<bool> ExistAsync(string? userName, Guid? id)
    {
        bool result = await _tenantRepository.ExistsAsync(userName, id);
        return result;
    }
}
