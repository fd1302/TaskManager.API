using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Project;

namespace FD.TaskManager.API.Controllers;

[Route("api/project")]
[ApiController]
public class ProjectController : ControllerBase
{
    private readonly ProjectManager _projectManager;
    private readonly AuthService _authService;
    public ProjectController(ProjectManager projectManager, AuthService authService)
    {
        _projectManager = projectManager ??
            throw new ArgumentNullException(nameof(projectManager));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    [Authorize(Roles = "Tenant, Admin")]
    [HttpPost("add")]
    public async Task<ActionResult<AddEntityResultDto>> Add(AddProjectDto addProjectDto)
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var info = _authService.GetUserInfoFromToken(token!);
        var result = await _projectManager.AddAsync(info, addProjectDto);
        return Ok(result);
    }
    [HttpGet("getprojects")]
    public async Task<IActionResult> GetProjects(string? searchQuery)
    {
        var result = await _projectManager.GetProjectsAsync(searchQuery);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet("getproject")]
    public async Task<IActionResult> GetProject(Guid id)
    {
        var result = await _projectManager.GetProjectAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpGet("getprojectswithtenantid")]
    public async Task<IActionResult> GetProjectsWithTenantId()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        if(token == null)
        {
            return Unauthorized();
        }
        var id = _authService.GetTenantIdFromToken(token);
        var result = await _projectManager.GetProjectsWithTenantIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpPatch("update")]
    public async Task<IActionResult> Update(Guid id, UpdateProjectDto updateProjectDto)
    {
        var result = await _projectManager.UpdateAsync(id, updateProjectDto);
        return result.Success is not false ? Ok(result) : NotFound(result.Message);
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _projectManager.DeleteAsync(id);
        return result.Success is not false ? Ok(result) : NotFound(result.Message);
    }
}
