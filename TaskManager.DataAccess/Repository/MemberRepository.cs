using Dapper;
using TaskManager.DataAccess.DbConnection;
using TaskManager.Shared.Entities;

namespace TaskManager.DataAccess.Repository;

public class MemberRepository
{
    private readonly DataBaseConnection _dbConnection;
    public MemberRepository(DataBaseConnection dbConnection)
    {
        _dbConnection = dbConnection ??
            throw new ArgumentNullException(nameof(dbConnection));
    }
    public async Task<bool> AddAsync(Member member)
    {
        string query =
            @"INSERT INTO Members
            VALUES (@Id, @TenantId, @UserName, @Email, @Password, @JoinedAt, @Role)";
        if (string.IsNullOrEmpty(member.TenantId.ToString()))
        {
            query =
                @"INSERT INTO Members
                VALUES (@Id, NULL, @UserName, @Email, @Password, @JoinedAt, @Role)";
        }
        var connection = _dbConnection.CreateConnection();
        int result = await connection.ExecuteAsync(query, new
        {
            member.Id,
            member.TenantId,
            member.UserName,
            member.Email,
            member.Password,
            member.JoinedAt,
            member.Role
        });
        return result > 0;
    }
    public async Task<bool> MemberExistsAsync(string? userName, Guid? id)
    {
        string query = @"";
        bool result;
        var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(userName))
        {
            query =
                @"SELECT CASE
                    WHEN EXISTS(SELECT 1 FROM Members WHERE UserName = @userName)
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
                    WHEN EXISTS(SELECT 1 FROM Members WHERE Id = @id)
                    THEN CAST(1 AS BIT)
                    ELSE CAST(0 AS BIT)
                END";
            result = await connection.ExecuteScalarAsync<bool>(query, new { id });
            return result;
        }
        return false;
    }
    public async Task<IEnumerable<Member>?> GetMembersAsync(string? searchQuery)
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
                FROM Members
                WHERE CONTAINS((UserName, Email), @searchQuery)";
            var result = await connection.QueryAsync<Member>(query, new { searchQuery });
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
            FROM Members";
        var managers = await connection.QueryAsync<Member>(query);
        return managers;
    }
    public async Task<Member?> GetMemberAsync(Guid id)
    {
        string query =
                @"SELECT
                    Id,
                    TenantId,
                    UserName,
                    Email,
                    JoinedAt,
                    Role
                FROM Members WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<Member?>(query, new { id });
        return result;
    }
    public async Task<Member?> GetMemberForAuthAsync(string userName)
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
            FROM Members
            WHERE UserName = @userName";
        var connection = _dbConnection.CreateConnection();
        var member = await connection.QuerySingleOrDefaultAsync<Member>(query, new { userName });
        return member;
    }
    public async Task<IEnumerable<Member>?> GetMembersWithTenantIdAsync(Guid id)
    {
        string query =
            @"SELECT
                M.Id,
                M.TenantId,
                M.UserName,
                M.Email,
                M.JoinedAt
            FROM Members M
            LEFT JOIN Tenants T ON T.Id = M.TenantId
            WHERE T.Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QueryAsync<Member>(query, new { id });
        return result;
    }
    public async Task<bool> JoinTenantAsync(Guid memberId, Guid tenantId)
    {
        string query =
            @"UPDATE Members
                SET TenantId = @tenantId
                WHERE Id = @memberId";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new
        {
            memberId,
            tenantId
        });
        return result > 0;
    }
    public async Task<bool> RemoveTenantMemberAsync(Guid id)
    {
        string query =
            @"UPDATE Members
            SET TenantId = NULL
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new { id });
        return result > 0;
    }
    public async Task<bool> UpdateAsync(Member member, Guid id)
    {
        var updates = new List<string>();
        if (!string.IsNullOrEmpty(member.UserName))
        {
            updates.Add("UserName = @UserName");
        }
        if (!string.IsNullOrEmpty(member.Email))
        {
            updates.Add("Email = @Email");
        }
        var connection = _dbConnection.CreateConnection();
        string query =
            $@"UPDATE Members
            SET {string.Join(", ", updates)}
            WHERE Id = @id";
        var affected = await connection.ExecuteAsync(query, new
        {
            id,
            member.UserName,
            member.Email
        });
        return affected > 0;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        string query = @"DELETE Members WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new { id });
        return result > 0;
    }
    public async Task<Member?> GetMemberForPromotionAsync(Guid id)
    {
        string query =
            @"SELECT 
                Id,
                UserName,
                Password,
                Email,
                JoinedAt
            FROM Members
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var member = await connection.QuerySingleOrDefaultAsync<Member>(query, new { id });
        return member;
    }
}
