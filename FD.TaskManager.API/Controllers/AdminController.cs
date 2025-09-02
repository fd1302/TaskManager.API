using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Admin;

namespace FD.TaskManager.API.Controllers;

[Route("api/admin")]
[ApiController]
public class AdminController : ControllerBase
{
    private readonly AdminManager _adminManager;
    private readonly AuthService _authService;
    public AdminController(AdminManager adminManager, AuthService authService)
    {
        _adminManager = adminManager ??
            throw new ArgumentNullException(nameof(adminManager));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    [Authorize(Roles = "Tenant, Admin")]
    [HttpPost("addadmin")]
    public async Task<IActionResult> AddAdmin(AddAdminDto addAdminDto)
    {
        try
        {
            Request.Cookies.TryGetValue("auth-token", out var token);
            if (token == null)
            {
                return Unauthorized();
            }
            var tenantId = _authService.GetIdFromToken(token);
            var result = await _adminManager.AddAsync(addAdminDto, tenantId);
            return Ok(result.Id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error while handling the request: {ex}");
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("getadmins")]
    public async Task<IActionResult> GetAdmins(string? searchQuery)
    {
        var result = await _adminManager.GetAdminsAsync(searchQuery);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet("getadmin")]
    public async Task<IActionResult> GetAdmin()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var id = _authService.GetIdFromToken(token!);
        var result = await _adminManager.GetAdminAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [Authorize(Roles = "Tenant")]
    [HttpGet("getadminswithtenantid")]
    public async Task<IActionResult> GetAdminsWithTenantId()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var id = _authService.GetIdFromToken(token!);
        var result = await _adminManager.GetAdminsWithTenantIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpPatch("updateadmin")]
    public async Task<IActionResult> Update(UpdateAdminDto updateAdminDto)
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var result = await _adminManager.UpdateAsync(updateAdminDto, id);
        return Ok(result);
    }
    [HttpDelete("deleteadmin")]
    public async Task<IActionResult> Delete()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var result = await _adminManager.DeleteAsync(id);
        if (result.Success)
        {
            Response.Cookies.Delete("auth-token");
        }
        return Ok(result);
    }
    [Authorize(Roles = "Tenant")]
    [HttpPost("promotion")]
    public async Task<IActionResult> PromotionToAdmin(PromotionDemotionDto promotionDto)
    {
        var result = await _adminManager.PromotionToAdminAsync(promotionDto);
        return Ok(result);
    }
}
