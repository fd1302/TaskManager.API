using Dapper;
using TaskManager.DataAccess.DbConnection;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Entities;

namespace TaskManager.DataAccess.Repository;

public class AdminRepository
{
    private readonly DataBaseConnection _dbConnection;
    public AdminRepository(DataBaseConnection dbConnection)
    {
        _dbConnection = dbConnection ??
            throw new ArgumentNullException(nameof(dbConnection));
    }
    public async Task<bool> AddAsync(Admin admin)
    {
        string query =
            @"INSERT INTO Admins
            VALUES (@Id, @TenantId, @UserName, @Email, @Password, @JoinedAt, @Role)";
        var connection = _dbConnection.CreateConnection();
        int result = await connection.ExecuteAsync(query, new
        {
            admin.Id,
            admin.TenantId,
            admin.UserName,
            admin.Email,
            admin.Password,
            admin.JoinedAt,
            admin.Role
        });
        return result > 0;
    }
    public async Task<bool> AdminUserNameExistsAsync(string userName)
    {
        string query =
            @"SELECT CASE 
                WHEN EXISTS(SELECT 1 FROM Admins WHERE UserName = @userName)
                THEN CAST(1 AS BIT)
                ELSE CAST(0 AS BIT)
            END";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteScalarAsync<bool>(query, new { userName });
        return result;
    }
    public async Task<IEnumerable<Admin>?> GetAdminsAsync(string? searchQuery)
    {
        string query = "";
        using var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query =
                @"SELECT
                    Id,
                    TenantId,
                    UserName.
                    Email,
                    JoinedAt,
                    Role
                FROM Admins
                WHERE CONTAINS((UserName, Email), @searchQuery)";
            var searchResult = await connection.QueryAsync<Admin>(query, new { searchQuery });
            return searchResult.ToList();
        }
        query = @"SELECT * FROM Admins";
        var admins = await connection.QueryAsync<Admin>(query);
        return admins;
    }
    public async Task<Admin?> GetAdminAsync(Guid id)
    {
        var query =
            @"SELECT
                Id,
                TenantId,
                UserName,
                Email,
                JoinedAt,
                Role
            FROM Admins
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var admin = await connection.QuerySingleOrDefaultAsync<Admin?>(query, new { id });
        return admin;
    }
    public async Task<Admin?> GetFullAdminInfoAsync(string? userName, Guid? id)
    {
        string query = @"";
        var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(userName))
        {
            query =
                @"SELECT
                    Id,
                    TenantId,
                    UserName,
                    Password,
                    Email,
                    JoinedAt,
                    Role
                FROM Admins
                WHERE UserName = @userName";
            var admin = await connection.QuerySingleOrDefaultAsync<Admin?>(query, new { userName });
            return admin;
        }
        else if (id != null)
        {
            query =
                @"SELECT
                    Id,
                    TenantId,
                    UserName,
                    Password,
                    Email,
                    JoinedAt,
                    Role
                FROM Admins
                WHERE id = @id";
            var admin = await connection.QuerySingleOrDefaultAsync<Admin?>(query, new { id });
            return admin;
        }
        return null;
    }
    public async Task<IEnumerable<Admin>?> GetAdminsWithTenantIdAsync(Guid id)
    {
        string query =
            @"SELECT
                A.Id,
                A.TenantId,
                A.UserName,
                A.Email,
                A.JoinedAt,
                A.Role
            FROM Admins A
            LEFT JOIN Tenants T ON T.Id = A.TenantId
            WHERE T.Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QueryAsync<Admin>(query, new { id });
        return result;
    }
    public async Task<bool> UpdateAsync(Admin admin, Guid id)
    {
        var updates = new List<string>();
        if (!string.IsNullOrEmpty(admin.UserName))
        {
            updates.Add("UserName = @UserName");
        }
        if (!string.IsNullOrEmpty(admin.Email))
        {
            updates.Add("Email = @Email");
        }
        string query = @$"
                UPDATE Admins
                SET {string.Join(", ", updates)}
                WHERE Id = @id
                ";
        var connection = _dbConnection.CreateConnection();
        var affected = await connection.ExecuteAsync(query, new
        {
            id,
            admin.UserName,
            admin.Email
        });
        return affected > 0;
    }
    public async Task<bool> ExistsAsync(Guid id)
    {
        string query =
            @"SELECT CASE
                WHEN EXISTS(SELECT 1 FROM Admins WHERE Id = @id)
                THEN CAST(1 AS BIT)
                ELSE CAST(0 AS BIT)
            END";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteScalarAsync<bool>(query, new { id });
        return result;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        string query = @"DELETE Admins WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new { id });
        return result > 0;
    }
}
