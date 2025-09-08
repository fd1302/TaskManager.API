using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Logic.AppManager;
using TaskManager.Shared.Dto_s.Board;

namespace FD.TaskManager.API.Controllers;

[Route("api/board")]
[ApiController]
public class BoardController : ControllerBase
{
    private readonly BoardManager _boardManager;
    public BoardController(BoardManager boardManager)
    {
        _boardManager = boardManager ??
            throw new ArgumentNullException(nameof(boardManager));
    }
    [HttpPost("add")]
    public async Task<IActionResult> Add(Guid projectId, AddBoardDto addBoardDto)
    {
        var result = await _boardManager.AddAsync(projectId, addBoardDto);
        return result.IsAdded is not false ? Ok($"IsAdded: {result.IsAdded}") : NotFound();
    }
    [HttpGet("getallboards")]
    public async Task<IActionResult> GetAllBoards()
    {
        var result = await _boardManager.GetAllBoardsAsync();
        return result is not null ? Ok(result) : NotFound();
    }
    [HttpGet("getboard")]
    public async Task<IActionResult> GetBoard(Guid id)
    {
        var result = await _boardManager.GetBoardAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpGet("getboardswithprojectid")]
    public async Task<IActionResult> GetBoardWithProjectId(Guid id)
    {
        var result = await _boardManager.GetBoardWithProjectIdAsync(id);
        return result is not null ? Ok(result) : NotFound();
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpPut("update")]
    public async Task<IActionResult> Update(Guid id, UpdateBoardDto updateBoardDto)
    {
        var result = await _boardManager.UpdateAsync(id, updateBoardDto);
        return Ok(result);
    }
    [Authorize(Roles = "Tenant, Admin, Manager")]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _boardManager.DeleteAsync(id);
        return Ok(result);
    }
}
