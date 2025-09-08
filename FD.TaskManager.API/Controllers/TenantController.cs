using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s.Tenant;

namespace FD.TaskManager.API.Controllers;

[Route("api/tenant")]
[ApiController]
public class TenantController : ControllerBase
{
    private readonly TenantManager _tenantManager;
    private readonly AuthService _authService;
    public TenantController(TenantManager tenantManager, AuthService authService)
    {
        _tenantManager = tenantManager ??
            throw new ArgumentNullException(nameof(tenantManager));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add(AddTenantDto model)
    {
        try
        {
            var result = await _tenantManager.AddAsync(model);
            Response.Cookies.Append("auth-token", result.Token!, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.Now.AddMinutes(5)
            });
            return result.IsAdded is not false ? Ok(result.Id) : NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error while trying to add new tenant: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("gettenants")]
    public async Task<IActionResult> GetTenants(string? searchQuery)
    {
        var result = await _tenantManager.GetTenantsAsync(searchQuery);
        return result is not null ? Ok(result) : NotFound();
    }
    [Authorize(Roles = "Tenant")]
    [HttpGet("gettenant")]
    public async Task<IActionResult> GetTenant()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var tenant = await _tenantManager.GetTenantAsync(id);
        return tenant is not null ? Ok(tenant) : NotFound("Wrong id or doesn't exist.");
    }
    [Authorize(Roles = "Tenant")]
    [HttpPatch("update")]
    public async Task<IActionResult> Update(UpdateTenantDto updateDto)
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var result = await _tenantManager.UpdateAsync(updateDto, id);
        return result.Success is not false ? Ok(result) : NotFound(result);
    }
    [Authorize(Roles = "Tenant")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var result = await _tenantManager.DeleteAsync(id);
        if (result.Success)
        {
            Response.Cookies.Delete("auth-token");
        }
        return result.Success is not false ? Ok(result) : NotFound(result);
    }
}
