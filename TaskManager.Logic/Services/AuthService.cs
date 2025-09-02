using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManager.DataAccess.Repository;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Authentication;

namespace TaskManager.Logic.Services;

public class AuthService
{
    private readonly TenantRepository _tenantRepository;
    private readonly AdminRepository _adminRepository;
    private readonly ManagerRepository _managerRepository;
    private readonly MemberRepository _memberRepository;
    private readonly PasswordHashing _passwordHashing;
    private readonly IConfiguration _configuration;
    public AuthService(TenantRepository tenantRepository, AdminRepository adminRepository, ManagerRepository managerRepository,
        MemberRepository memberRepository, PasswordHashing passwordHashing, IConfiguration configuration)
    {
        _tenantRepository = tenantRepository ??
            throw new ArgumentNullException(nameof(tenantRepository));
        _adminRepository = adminRepository ??
            throw new ArgumentNullException(nameof(adminRepository));
        _managerRepository = managerRepository ??
            throw new ArgumentNullException(nameof(managerRepository));
        _memberRepository = memberRepository ??
            throw new ArgumentNullException(nameof(memberRepository));
        _passwordHashing = passwordHashing ??
            throw new ArgumentNullException(nameof(passwordHashing));
        _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));
    }
    public async Task<AuthenticationResultDto> TenantAuthenticationAsync(AuthenticationDto authenticationDto)
    {
        var tenant = await _tenantRepository.GetTenantForAuthAsync(authenticationDto.UserName);
        if (tenant == null)
        {
            return new AuthenticationResultDto()
            {
                ErrorMessage = "User not found."
            };
        }
        bool checkPass = _passwordHashing.Verify(authenticationDto.Password, tenant.Password);
        if (!checkPass)
        {
            return new AuthenticationResultDto()
            {
                ErrorMessage = "Wrong password."
            };
        }
        var jwtKey = _configuration["JwtSettings:Key"];
        if (jwtKey == null)
        {
            throw new Exception("Jwt key is not figured.");
        }
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        var tokenClaims = new List<Claim>()
        {
            new Claim("id", tenant.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Name, tenant.Name),
            new Claim(JwtRegisteredClaimNames.PreferredUsername, tenant.UserName),
            new Claim("role", tenant.Role)
        };
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            tokenClaims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(30),
            signingCredentials
            );
        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);
        return new AuthenticationResultDto
        {
            IsSuccessful = true,
            Token = token
        };
    }
    public async Task<AuthenticationResultDto> AdminAuthenticationAsync(AuthenticationDto authenticationDto)
    {
        var admin = await _adminRepository.GetFullAdminInfoAsync(authenticationDto.UserName, null);
        if (admin == null)
        {
            return new AuthenticationResultDto()
            {
                ErrorMessage = "Admin not found."
            };
        }
        bool checkPass = _passwordHashing.Verify(authenticationDto.Password, admin.Password);
        if (!checkPass)
        {
            return new AuthenticationResultDto()
            {
                ErrorMessage = "Wrong password."
            };
        }
        var jwtKey = _configuration["JwtSettings:Key"];
        if (jwtKey == null)
        {
            throw new Exception("Jwt key is not figured.");
        }
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        var tokenClaims = new List<Claim>()
        {
            new Claim("id", admin.Id.ToString()),
            new Claim("tenantId", admin.TenantId.ToString()),
            new Claim(JwtRegisteredClaimNames.PreferredUsername, admin.UserName),
            new Claim(JwtRegisteredClaimNames.Email, admin.Email),
            new Claim("role", admin.Role)
        };
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            tokenClaims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(30),
            signingCredentials
            );
        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);
        return new AuthenticationResultDto
        {
            IsSuccessful = true,
            Token = token
        };
    }
    public async Task<AuthenticationResultDto> ManagerAuthenticationAsync(AuthenticationDto authenticationDto)
    {
        var manager = await _managerRepository.GetManagerWithUserNameAsync(authenticationDto.UserName);
        if (manager == null)
        {
            return new AuthenticationResultDto()
            {
                ErrorMessage = "Manager not found."
            };
        }
        bool checkPass = _passwordHashing.Verify(authenticationDto.Password, manager.Password);
        if (!checkPass)
        {
            return new AuthenticationResultDto()
            {
                ErrorMessage = "Wrong password."
            };
        }
        var jwtKey = _configuration["JwtSettings:Key"];
        if (jwtKey == null)
        {
            throw new Exception("Jwt key is not figured.");
        }
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        var tokenClaims = new List<Claim>()
        {
            new Claim("id", manager.Id.ToString()),
            new Claim("tenantId", manager.TenantId.ToString()),
            new Claim(JwtRegisteredClaimNames.PreferredUsername, manager.UserName),
            new Claim(JwtRegisteredClaimNames.Email, manager.Email),
            new Claim("role", manager.Role)
        };
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            tokenClaims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(30),
            signingCredentials
            );
        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);
        return new AuthenticationResultDto
        {
            IsSuccessful = true,
            Token = token
        };
    }
    public async Task<AuthenticationResultDto> MemberAuthenticationAsync(AuthenticationDto authenticationDto)
    {
        var member = await _memberRepository.GetMemberForAuthAsync(authenticationDto.UserName);
        if (member == null)
        {
            return new AuthenticationResultDto()
            {
                ErrorMessage = "Member not found."
            };
        }
        var checkPass = _passwordHashing.Verify(authenticationDto.Password, member.Password);
        if (!checkPass)
        {
            return new AuthenticationResultDto()
            {
                ErrorMessage = "Wrong password."
            };
        }
        var jwtKey = _configuration["JwtSettings:Key"];
        if (jwtKey == null)
        {
            throw new Exception("Jwt key is not figured.");
        }
        var securityKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtKey));
        var signingCredentials = new SigningCredentials(
            securityKey, SecurityAlgorithms.HmacSha256);
        var tokenClaims = new List<Claim>()
        {
            new Claim("id", member.Id.ToString()),
            new Claim("tenantId", member.TenantId.ToString()),
            new Claim(JwtRegisteredClaimNames.PreferredUsername, member.UserName),
            new Claim(JwtRegisteredClaimNames.Email, member.Email),
            new Claim("role", member.Role)
        };
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["JwtSettings:Issuer"],
            _configuration["JwtSettings:Audience"],
            tokenClaims,
            DateTime.UtcNow,
            DateTime.UtcNow.AddMinutes(30),
            signingCredentials
            );
        var token = new JwtSecurityTokenHandler()
            .WriteToken(jwtSecurityToken);
        return new AuthenticationResultDto
        {
            IsSuccessful = true,
            Token = token
        };
    }
    public Guid GetIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var id = jwtToken.Claims.FirstOrDefault(s => s.Type == "id")!.ToString().Replace("id:", "").Trim();
        return Guid.Parse(id);
    }
    public Guid GetTenantIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var roleName = jwtToken.Claims.FirstOrDefault(s => s.Type == "role")!.ToString().Replace("role:", "").Trim();
        if(roleName == "Tenant")
        {
            var id = jwtToken.Claims.FirstOrDefault(s => s.Type == "id")!.ToString().Replace("id:", "").Trim();
            return Guid.Parse(id);
        }
        var tenantId = jwtToken.Claims.FirstOrDefault(s => s.Type == "tenantId")!.ToString().Replace("tenantId:", "").Trim();
        return Guid.Parse(tenantId);
    }
    public UserTokenInfoDto GetUserInfoFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);
        var role = jwtToken.Claims.FirstOrDefault(s => s.Type == "role")!.ToString().Replace("role:", "").Trim();
        var tenantId = jwtToken.Claims.FirstOrDefault(s => s.Type == "tenantId")?.ToString().Replace("tenantId:", "").Trim();
        var userTokenInfo = new UserTokenInfoDto();
        if(role != "Tenant" && tenantId != null)
        {
            userTokenInfo.TenantId = Guid.Parse(tenantId);
        }
        userTokenInfo.Id = GetIdFromToken(token);
        userTokenInfo.UserName = jwtToken.Claims.FirstOrDefault(s => s.Type == "preferred_username")!.ToString().Replace("preferred_username:", "").Trim();
        userTokenInfo.Role = role;
        return userTokenInfo;
    }
}
