using TaskManager.Shared.Dto_s.Board;
using TaskManager.Shared.Entities;

namespace TaskManager.Logic.Mapping;

public class BoardMapping
{
    public Board AddBoardDtoTOBoard(AddBoardDto addBoardDto)
    {
        return new Board()
        {
            Description = addBoardDto.Description
        };
    }
    public BoardDto BoardTOBoardDto(Board board)
    {
        return new BoardDto()
        {
            Id = board.Id,
            ProjectId = board.ProjectId,
            Description = board.Description,
            CreatedAt = board.CreatedAt
        };
    }
    public Board UpdateBoardDtoTOBoard(UpdateBoardDto updateBoardDto)
    {
        return new Board()
        {
            Description = updateBoardDto.Description
        };
    }
}
