using Dapper;
using TaskManager.DataAccess.DbConnection;
using TaskManager.Shared.Entities;

namespace TaskManager.DataAccess.Repository;

public class TaskItemRepository
{
    private readonly DataBaseConnection _dbConnection;
    public TaskItemRepository(DataBaseConnection dbConnection)
    {
        _dbConnection = dbConnection ??
            throw new ArgumentNullException(nameof(dbConnection));
    }
    public async Task<bool> AddAsync(TaskItem task)
    {
        string query =
            @"INSERT INTO TaskItems
            VALUES (@Id, @BoardId, @Title, @Description, @AssignedMemberId, @AssignedMemberName, @Status, @CreatedAt)";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new
        {
            task.Id,
            task.BoardId,
            task.Title,
            task.Description,
            task.AssignedMemberId,
            task.AssignedMemberName,
            task.Status,
            task.CreatedAt
        });
        return result > 0;
    }
    public async Task<IEnumerable<TaskItem>?> GetTaskItemsAsync(string? searchQuery)
    {
        string query = @"";
        var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query =
                @"SELECT
                    Id,
                    BoardId,
                    Title,
                    Description,
                    AssignedMemberId,
                    AssignedMemberName,
                    Status,
                    CreatedAt
                FROM TaskItems
                WHERE CONTAINS((Title, Description, AssignedMemberId), @searchQuery)";
            var searchResult = await connection.QueryAsync<TaskItem>(query, new { searchQuery });
            return searchResult;
        }
        query =
            @"SELECT
                Id,
                BoardId,
                Title,
                Description,
                AssignedMemberId,
                AssignedMemberName,
                Status,
                CreatedAt
            FROM TaskItems";
        var taskItems = await connection.QueryAsync<TaskItem>(query);
        return taskItems;
    }
    public async Task<TaskItem?> GetTaskItemAsync(Guid id)
    {
        string query =
            @"SELECT
                Id,
                BoardId,
                Title,
                Description,
                AssignedMemberId,
                AssignedMemberName,
                Status,
                CreatedAt
            FROM TaskItems
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var taskItem = await connection.QuerySingleOrDefaultAsync<TaskItem>(query, new { id });
        return taskItem;
    }
    public async Task<IEnumerable<TaskItem>?> GetTaskItemsWithBoardIdAsync(Guid boardId)
    {
        string query =
            @"SELECT
                TI.Id,
                TI.BoardId,
                TI.Title,
                TI.Description,
                TI.AssignedMemberId,
                TI.AssignedMemberName,
                TI.Status,
                TI.CreatedAt
            FROM TaskItems TI
            JOIN Boards B ON B.Id = TI.BoardId
            WHERE TI.BoardId = @boardId";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QueryAsync<TaskItem>(query, new { boardId });
        return result;
    }
    public async Task<IEnumerable<TaskItem>?> GetTaskItemsWithMemberIdAsync(Guid id)
    {
        string query =
            @"SELECT
                Id,
                BoardId,
                Title,
                Description,
                AssignedMemberId,
                AssignedMemberName,
                Status,
                CreatedAt
            FROM TaskItems
            WHERE AssignedMemberId = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QueryAsync<TaskItem>(query, new { id });
        return result;
    }
    public async Task<bool> UpdateAsync(Guid id, TaskItem taskItem)
    {
        var updates = new List<string>();
        if (!string.IsNullOrEmpty(taskItem.Title))
        {
            updates.Add("Title = @Title");
        }
        if (!string.IsNullOrEmpty(taskItem.Description))
        {
            updates.Add("Description = @Description");
        }
        if (!string.IsNullOrEmpty(taskItem.Status))
        {
            updates.Add("Status = @Status");
        }
        string query =
            $@"UPDATE TaskItems
            SET {string.Join(", ", updates)}
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new
        {
            id,
            taskItem.Title,
            taskItem.Description,
            taskItem.Status
        });
        return result > 0;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        string query =
            @"DELETE TaskItems
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new { id });
        return result > 0;
    }
    public async Task<bool> TaskItemExistsAsync(Guid id)
    {
        string query =
            @"SELECT CASE
                WHEN EXISTS(SELECT 1 FROM TaskItems WHERE Id = @id)
                THEN CAST (1 AS BIT)
                ELSE CAST (0 AS BIT)
            END";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteScalarAsync<bool>(query, new { id });
        return result;
    }
}
