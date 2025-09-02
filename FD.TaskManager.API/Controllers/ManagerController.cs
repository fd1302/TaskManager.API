using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Manager;

namespace FD.TaskManager.API.Controllers;

[Route("api/manager")]
[ApiController]
public class ManagerController : ControllerBase
{
    private readonly MManager _mManager;
    private readonly AuthService _authService;
    public ManagerController(MManager mManager, AuthService authService)
    {
        _mManager = mManager ??
            throw new ArgumentNullException(nameof(mManager));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    [HttpPost("addmanager")]
    public async Task<IActionResult> Add(AddManagerDto addManagerDto)
    {
        try
        {
            var result = await _mManager.AddAsync(addManagerDto);
            Response.Cookies.Append("auth-token", result.Token!, new CookieOptions()
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(5)
            });
            return result.IsAdded is not false ? Ok(result.Id) : NotFound();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There was an error while handling the request: {ex}");
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("getmanagers")]
    public async Task<IActionResult> GetManagers(string? searchQuery)
    {
        var result = await _mManager.GetManagersAsync(searchQuery);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet("getmanager")]
    public async Task<IActionResult> GetManager()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var id = _authService.GetIdFromToken(token!);
        var result = await _mManager.GetManagerAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet("getmanagerswithtenantid")]
    public async Task<IActionResult> GetManagersWithTenantId()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var info = _authService.GetUserInfoFromToken(token!);
        var result = await _mManager.GetManagersWithTenantIdAsync(info);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpPatch("updatemanager")]
    public async Task<IActionResult> Update(UpdateManagerDto updateManagerDto)
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var id = _authService.GetIdFromToken(token!);
        var result = await _mManager.UpdateAsync(updateManagerDto, id);
        return Ok(result);
    }
    [HttpDelete("deletemanager")]
    public async Task<IActionResult> Delete()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var result = await _mManager.DeleteAsync(id);
        if (result.Success)
        {
            Response.Cookies.Delete("auth-token");
        }
        return Ok(result);
    }
    [Authorize(Roles = "Tenant")]
    [HttpPost("promotion")]
    public async Task<IActionResult> PromotionToManager(PromotionDemotionDto promotionDto)
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var id = _authService.GetIdFromToken(token!);
        var result = await _mManager.PromotionToManagerAsync(promotionDto, id);
        return Ok(result);
    }
    [Authorize(Roles = "Tenant")]
    [HttpPost("demotion")]
    public async Task<IActionResult> DemotionToManager(PromotionDemotionDto demotionDto)
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var id = _authService.GetIdFromToken(token!);
        var result = await _mManager.DemotionToManagerAsync(demotionDto, id);
        return Ok(result);
    }
}
