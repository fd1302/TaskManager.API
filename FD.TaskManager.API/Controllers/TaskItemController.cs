using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Shared.Dto_s.TaskItem;

namespace FD.TaskManager.API.Controllers;

[Route("api/taskitem")]
[ApiController]
public class TaskItemController : ControllerBase
{
    private readonly TaskItemManager _taskItemManager;
    public TaskItemController(TaskItemManager taskItemManager)
    {
        _taskItemManager = taskItemManager ??
            throw new ArgumentNullException(nameof(taskItemManager));
    }
    [HttpPost]
    public async Task<IActionResult> Add(AddTaskItemDto addTaskItemDto)
    {
        var result = await _taskItemManager.AddAsync(addTaskItemDto);
        return Ok(result);
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
    [HttpGet("gettaskitemwithboardid")]
    public async Task<IActionResult> GetTaskItemWithBoardId(Guid id)
    {
        var result = await _taskItemManager.GetTaskItemsWithBoardIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpPatch("updatetaskitem")]
    public async Task<IActionResult> Update(Guid id, UpdateTaskItemDto updateTaskItemDto)
    {
        var result = await _taskItemManager.UpdateAsync(id, updateTaskItemDto);
        return result.Success is not false ? Ok(result) : NotFound(result);
    }
    [HttpDelete("deletetaskitem")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _taskItemManager.DeleteAsync(id);
        return result.Success is not false ? Ok(result) : NotFound(result.Message);
    }
}
