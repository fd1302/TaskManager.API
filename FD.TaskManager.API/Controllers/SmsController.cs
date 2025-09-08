using Microsoft.AspNetCore.Mvc;
using Siahroud.Logic.Services;
using TaskManager.Logic.AppManager;
using TaskManager.Shared.Dto_s;

namespace FD.TaskManager.API.Controllers;

[Route("api/sms")]
[ApiController]
public class SmsController : ControllerBase
{
    private readonly SMSService _smsService;
    private readonly TenantManager _tenantManager;
    public SmsController(SMSService smsService, TenantManager tenantManager)
    {
        _smsService = smsService ??
            throw new ArgumentNullException(nameof(smsService));
        _tenantManager = tenantManager ??
            throw new ArgumentNullException(nameof(tenantManager));
    }
    [HttpPost("tenantpassrecovery")]
    public async Task<IActionResult> TenantPassRecovery(ChangePassCodeRequestDto codeRequestDto)
    {
        var tenant = await _tenantManager.ExistAsync(codeRequestDto.UserName, null);
        if (!tenant)
        {
            return NotFound();
        }
        var result = await _smsService.PassRecoveryMessageAsync(codeRequestDto.PhoneNumber);
        Response.Cookies.Append("phoneNumber", codeRequestDto.PhoneNumber, new CookieOptions()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddMinutes(5)
        });
        return result is not null ? Ok(result) : NotFound(result);
    }
    [HttpGet("tenantcheckcode")]
    public IActionResult TenantCheckCode(string code)
    {
        string phoneNumber = Request.Cookies["phoneNumber"]!;
        if (string.IsNullOrEmpty(phoneNumber))
        {
            return BadRequest("Phone number cookie is missing.");
        }
        bool result = _smsService.CheckCode(phoneNumber, code);
        string token = "";
        if (result)
        {
            token = _smsService.PasswordRercoveryPageToken(phoneNumber);
        }
        return result is not false ? Ok() : NotFound();
    }
}
