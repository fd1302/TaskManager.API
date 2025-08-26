using Dapper;
using TaskManager.DataAccess.DbConnection;
using TaskManager.Shared.Dto_s.Board;
using TaskManager.Shared.Entities;

namespace TaskManager.DataAccess.Repository;

public class BoardRepository
{
    private readonly DataBaseConnection _dbConnection;
    public BoardRepository(DataBaseConnection dbConnection)
    {
        _dbConnection = dbConnection ??
            throw new ArgumentNullException(nameof(dbConnection));
    }
    public async Task<bool> AddAsync(Board board)
    {
        string query =
            @"INSERT INTO Boards
            VALUES (@Id, @ProjectId, @Description, @CreatedAt)";
        var connection = _dbConnection.CreateConnection();
        int result = await connection.ExecuteAsync(query, new
        {
            board.Id,
            board.ProjectId,
            board.Description,
            board.CreatedAt
        });
        return result > 0;
    }
    public async Task<IEnumerable<Board>?> GetAllBoardsAsync()
    {
        string query =
            @"SELECT
                Id,
                ProjectId,
                Description,
                CreatedAt
            FROM Boards";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QueryAsync<Board>(query);
        return result;
    }
    public async Task<Board?> GetBoardAsync(Guid id)
    {
        string query =
            @"SELECT 
                Id,
                ProjectId,
                Description,
                CreatedAt
            FROM Boards
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var board = await connection.QuerySingleOrDefaultAsync<Board?>(query, new { id });
        return board;
    }
    public async Task<IEnumerable<Board>?> GetBoardWithProjectIdAsync(Guid projectId)
    {
        string query =
            @"SELECT B.Id, B.ProjectId, B.Description, B.CreatedAt
            FROM Boards B
            JOIN Projects P ON B.ProjectId = P.Id
            WHERE B.ProjectId = @projectId";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QueryAsync<Board>(query, new { projectId });
        return result;
    }
    public async Task<bool> UpdateAsync(Guid id, Board board)
    {
        if (string.IsNullOrEmpty(board.Description))
        {
            return true;
        }
        string query =
            @"UPDATE Boards
            SET Description = @Description
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new
        {
            id,
            board.Description
        });
        return result > 0;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        string query =
            @"DELETE Boards
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new { id });
        return result > 0;
    }
    public async Task<bool> BoardExistsAsync(Guid id)
    {
        string query =
            @"SELECT CASE
                WHEN EXISTS(SELECT 1 FROM Boards WHERE Id = @id)
                THEN CAST (1 AS BIT)
                ELSE CAST (0 AS BIT)
            END";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteScalarAsync<bool>(query, new { id });
        return result;
    }
}
