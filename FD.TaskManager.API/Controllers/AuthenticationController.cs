using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Admin;
using TaskManager.Shared.Dto_s.Authentication;

namespace FD.TaskManager.API.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly AuthService _authService;
    public AuthenticationController(AuthService authService)
    {
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    [HttpPost("tenantlogin")]
    public async Task<IActionResult> TenantLogin(AuthenticationDto authenticationDto)
    {
        var result = await _authService.TenantAuthenticationAsync(authenticationDto);
        if (!result.IsSuccessful)
        {
            return BadRequest(result);
        }
        Response.Cookies.Append("auth-token", result.Token!, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddMinutes(30)
        });
        return Ok(result);
    }
    [HttpPost("adminlogin")]
    public async Task<IActionResult> AdminLogin(AuthenticationDto authenticationDto)
    {
        var result = await _authService.AdminAuthenticationAsync(authenticationDto);
        if (!result.IsSuccessful)
        {
            return Unauthorized(result.ErrorMessage);
        }
        Response.Cookies.Append("auth-token", result.Token!, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddMinutes(30)
        });
        return Ok(result);
    }
    [HttpPost("managerlogin")]
    public async Task<IActionResult> ManagerLogin(AuthenticationDto authenticationDto)
    {
        var result = await _authService.ManagerAuthenticationAsync(authenticationDto);
        if (!result.IsSuccessful)
        {
            return Unauthorized(result.ErrorMessage);
        }
        Response.Cookies.Append("auth-token", result.Token!, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.Now.AddMinutes(30)
        });
        return Ok(result);
    }
    [HttpPost("memberlogin")]
    public async Task<IActionResult> MemberLogin(AuthenticationDto authenticationDto)
    {
        var result = await _authService.MemberAuthenticationAsync(authenticationDto);
        if (!result.IsSuccessful)
        {
            return Unauthorized(result.ErrorMessage);
        }
        Response.Cookies.Append("auth-token", result.Token!, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddMinutes(30)
        });
        return Ok(result);
    }
    // Test
    [HttpGet("senduserinfo")]
    public IActionResult SendUserInfo()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var result = _authService.GetUserInfoFromToken(token);
        return Ok(result);
    }
    [Authorize]
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("auth-token");
        return Ok();
    }
}
