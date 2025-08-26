using TaskManager.DataAccess.Repository;
using TaskManager.Logic.Mapping;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Board;

namespace TaskManager.Logic.AppManager;

public class BoardManager
{
    private readonly BoardRepository _boardRepository;
    private readonly BoardMapping _boardMapping;
    public BoardManager(BoardRepository boardRepository, BoardMapping boardMapping)
    {
        _boardRepository = boardRepository ??
            throw new ArgumentNullException(nameof(boardRepository));
        _boardMapping = boardMapping ??
            throw new ArgumentNullException(nameof(boardMapping));
    }
    public async Task<AddEntityResultDto> AddAsync(Guid projectId, AddBoardDto addBoardDto)
    {
        if (addBoardDto.Description == null)
        {
            throw new ArgumentNullException("Description can't be empty.");
        }
        else if (addBoardDto.Description.Length > 300)
        {
            throw new ArgumentException("Descriptin can't be more than 300 characters.");
        }
        var board = _boardMapping.AddBoardDtoTOBoard(addBoardDto);
        board.Id = Guid.NewGuid();
        board.ProjectId = projectId;
        board.CreatedAt = DateTime.UtcNow.ToString().Substring(0, 9);
        bool result = await _boardRepository.AddAsync(board);
        if (!result)
        {
            return new AddEntityResultDto();
        }
        return new AddEntityResultDto()
        {
            IsAdded = true
        };
    }
    public async Task<IEnumerable<BoardDto>?> GetAllBoardsAsync()
    {
        var boards = await _boardRepository.GetAllBoardsAsync();
        if (boards == null)
        {
            return null;
        }
        var mappedBoards = boards.Select(_boardMapping.BoardTOBoardDto);
        return mappedBoards;
    }
    public async Task<BoardDto?> GetBoardAsync(Guid id)
    {
        var board = await _boardRepository.GetBoardAsync(id);
        if (board == null)
        {
            return null;
        }
        var mappedBoard = _boardMapping.BoardTOBoardDto(board);
        return mappedBoard;
    }
    public async Task<IEnumerable<BoardDto>?> GetBoardWithProjectIdAsync(Guid projectId)
    {
        var result = await _boardRepository.GetBoardWithProjectIdAsync(projectId);
        if (result == null)
        {
            return null;
        }
        var mappedBoards = result.Select(_boardMapping.BoardTOBoardDto);
        return mappedBoards;
    }
    public async Task<ResultDto> UpdateAsync(Guid id, UpdateBoardDto updateBoardDto)
    {
        bool boardExists = await _boardRepository.BoardExistsAsync(id);
        if (!boardExists)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "Board not found."
            };
        }
        else if (updateBoardDto.Description != null)
        {
            if (updateBoardDto.Description.Length > 300)
            {
                throw new ArgumentException("Description can't be more than 300 characters.");
            }
        }
        var board = _boardMapping.UpdateBoardDtoTOBoard(updateBoardDto);
        var result = await _boardRepository.UpdateAsync(id, board);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Update",
                Message = "There was an error while handling the request."
            };
        }
        return new ResultDto()
        {
            Success = true,
            Operation = "Update",
            Message = "Updated successfuly."
        };
    }
    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        bool boardExists = await _boardRepository.BoardExistsAsync(id);
        if (!boardExists)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "Board not found."
            };
        }
        bool result = await _boardRepository.DeleteAsync(id);
        if (!result)
        {
            return new ResultDto()
            {
                Operation = "Delete",
                Message = "There was an error while handling the request."
            };
        }
        return new ResultDto()
        {
            Success = true,
            Operation = "Delete",
            Message = "Deleted Successfuly."
        };
    }
}
