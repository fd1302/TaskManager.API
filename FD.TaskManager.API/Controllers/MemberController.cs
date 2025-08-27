using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s.Member;

namespace FD.TaskManager.API.Controllers;

[Route("api/member")]
[ApiController]
public class MemberController : ControllerBase
{
    private readonly MemberManager _memberManager;
    private readonly AuthService _authService;
    public MemberController(MemberManager memberManager, AuthService authService)
    {
        _memberManager = memberManager ??
            throw new ArgumentNullException(nameof(memberManager));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    [HttpPost("addmember")]
    public async Task<IActionResult> Add(AddMemberDto addMemberDto)
    {
        try
        {
            var result = await _memberManager.AddAsync(addMemberDto);
            if (result.IsAdded != false & result.Token != null)
            {
                Response.Cookies.Append("auth-token", result.Token, new CookieOptions()
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                    Expires = DateTime.UtcNow.AddMinutes(5)
                });
            }
            return Ok(result);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while adding member: {ex.Message}");
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("getmemebers")]
    public async Task<IActionResult> GetMembers(string? searchQuery)
    {
        var result = await _memberManager.GetMembersAsync(searchQuery);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet("getmember")]
    public async Task<IActionResult> GetMember()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if(token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var result = await _memberManager.GetMemberAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpPatch("updatemember")]
    public async Task<IActionResult> Update(UpdateMemberDto updateMemberDto)
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var result = await _memberManager.UpdateAsync(updateMemberDto, id);
        return Ok(result);
    }
    [HttpDelete("deletmember")]
    public async Task<IActionResult> Delete()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if (token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetIdFromToken(token);
        var result = await _memberManager.DeleteAsync(id);
        if (result.Success)
        {
            Response.Cookies.Delete("auth-token");
        }
        return Ok(result);
    }
}
