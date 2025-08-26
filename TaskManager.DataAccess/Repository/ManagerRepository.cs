using Dapper;
using TaskManager.DataAccess.DbConnection;
using TaskManager.Shared.Dto_s.Manager;
using TaskManager.Shared.Entities;

namespace TaskManager.DataAccess.Repository;

public class ManagerRepository
{
    private readonly DataBaseConnection _dbConnection;
    public ManagerRepository(DataBaseConnection dbConnection)
    {
        _dbConnection = dbConnection ??
            throw new ArgumentNullException(nameof(dbConnection));
    }
    public async Task<bool> AddAsync(Manager manager)
    {
        var query =
                @"INSERT INTO Managers
                VALUES (@Id, @TenantId, @UserName, @Email, @Password, @JoinedAt, @Role)";
        var connection = _dbConnection.CreateConnection();
        int result = await connection.ExecuteAsync(query, new
        {
            manager.Id,
            manager.TenantId,
            manager.UserName,
            manager.Email,
            manager.Password,
            manager.JoinedAt,
            manager.Role
        });
        return result > 0;
    }
    public async Task<bool> ExistsAsync(string? userName, Guid? id)
    {
        string query = @"";
        bool result;
        var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(userName))
        {
            query =
                @"SELECT CASE
                    WHEN EXISTS(SELECT 1 FROM Managers WHERE UserName = @userName)
                    THEN CAST(1 AS BIT)
                    ELSE CAST(0 AS BIT)
                END";
            result = await connection.ExecuteScalarAsync<bool>(query, new { userName });
            return result;
        }
        else if (id != null)
        {
            query =
                @"SELECT CASE
                    WHEN EXISTS(SELECT 1 FROM Managers WHERE Id = @id)
                    THEN CAST(1 AS BIT)
                    ELSE CAST(0 AS BIT)
                END";
            result = await connection.ExecuteScalarAsync<bool>(query, new { id });
            return result;
        }
        return false;
    }
    public async Task<IEnumerable<Manager>?> GetManagersAsync(string? searchQuery)
    {
        string query = @"";
        var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query =
                @"SELECT
                    Id,
                    TenantId,
                    UserName,
                    Email,
                    JoinedAt,
                    Role
                FROM Managers
                WHERE CONTAINS((UserName, Email), @searchQuery)";
            var result = await connection.QueryAsync<Manager>(query, new { searchQuery });
            return result;
        }
        query =
            @"SELECT
                Id,
                TenantId,
                UserName,
                Email,
                JoinedAt,
                Role
            FROM Managers";
        var managers = await connection.QueryAsync<Manager>(query);
        return managers;
    }
    public async Task<Manager?> GetManagerAsync(Guid id)
    {
        string query =
            @"SELECT
                Id,
                TenantId,
                UserName,
                Email,
                JoinedAt,
                Role
            FROM Managers WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var manager = await connection.QuerySingleOrDefaultAsync<Manager?>(query, new { id });
        return manager;
    }
    public async Task<Manager?> GetManagerWithUserNameAsync(string userName)
    {
        string query =
            @"SELECT
                Id,
                TenantId,
                UserName,
                Password,
                Email,
                JoinedAt,
                Role
            FROM Managers
            WHERE UserName = @userName";
        var connection = _dbConnection.CreateConnection();
        var manager = await connection.QuerySingleOrDefaultAsync<Manager?>(query, new { userName });
        return manager;
    }
    public async Task<bool> UpdateAsync(Manager manager, Guid id)
    {
        var updates = new List<string>();
        if (!string.IsNullOrEmpty(manager.UserName))
        {
            updates.Add("UserName = @UserName");
        }
        if (!string.IsNullOrEmpty(manager.Email))
        {
            updates.Add("Email = @Email");
        }
        var connection = _dbConnection.CreateConnection();
        string query =
            @$"UPDATE Managers
            SET {string.Join(", ", updates)}
            WHERE Id = @id";
        var affected = await connection.ExecuteAsync(query, new
        {
            id,
            manager.UserName,
            manager.Email
        });
        return affected > 0;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        string query = @"DELETE Managers WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var affected = await connection.ExecuteAsync(query, new { id });
        return affected > 0;
    }
    public async Task<Manager?> GetManagerForPromotionAsync(Guid id)
    {
        string query =
            @"SELECT
                Id,
                TenantId,
                UserName,
                Password,
                Email,
                JoinedAt
            FROM Managers
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var manager = await connection.QuerySingleOrDefaultAsync<Manager>(query, new { id });
        return manager;
    }
}
