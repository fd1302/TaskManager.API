using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Logic.Services;
using TaskManager.Shared.Dto_s.TaskItem;

namespace FD.TaskManager.API.Controllers;

[Route("api/taskitem")]
[ApiController]
public class TaskItemController : ControllerBase
{
    private readonly TaskItemManager _taskItemManager;
    private readonly AuthService _authService;
    public TaskItemController(TaskItemManager taskItemManager, AuthService authService)
    {
        _taskItemManager = taskItemManager ??
            throw new ArgumentNullException(nameof(taskItemManager));
        _authService = authService ??
            throw new ArgumentNullException(nameof(authService));
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpPost("add")]
    public async Task<IActionResult> Add(AddTaskItemDto addTaskItemDto)
    {
        try
        {
            var result = await _taskItemManager.AddAsync(addTaskItemDto);
            return Ok(result);
        }
        catch(Exception ex)
        {
            Console.WriteLine(ex.Message);
            return NotFound();
        }
    }
    [HttpGet("gettaskitems")]
    public async Task<IActionResult> GetTaskItems(string? searchQuery)
    {
        var result = await _taskItemManager.GetTaskItemsAsync(searchQuery);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet("gettaskitem")]
    public async Task<IActionResult> GetTaskItem(Guid id)
    {
        var result = await _taskItemManager.GetTaskItemAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpGet("gettaskitemwithboardid")]
    public async Task<IActionResult> GetTaskItemWithBoardId(Guid id)
    {
        var result = await _taskItemManager.GetTaskItemsWithBoardIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [Authorize(Roles = "Member")]
    [HttpGet("gettaskitemswithmemberid")]
    public async Task<IActionResult> GetTaskItemsWithMemberId()
    {
        Request.Cookies.TryGetValue("auth-token", out var token);
        var id = _authService.GetIdFromToken(token!);
        var result = await _taskItemManager.GetTaskItemsWithMemberIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpPatch("update")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskItemDto updateTaskItemDto)
    {
        var result = await _taskItemManager.UpdateAsync(id, updateTaskItemDto);
        return result.Success is not false ? Ok(result) : NotFound(result);
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _taskItemManager.DeleteAsync(id);
        return result.Success is not false ? Ok(result) : NotFound(result);
    }
}
