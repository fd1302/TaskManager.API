using Dapper;
using TaskManager.DataAccess.DbConnection;
using TaskManager.Shared.Dto_s;
using TaskManager.Shared.Dto_s.Tenant;
using TaskManager.Shared.Entities;

namespace TaskManager.DataAccess.Repository;

public class TenantRepository
{
    private readonly DataBaseConnection _dbConnection;
    public TenantRepository(DataBaseConnection dbConnection)
    {
        _dbConnection = dbConnection ??
            throw new ArgumentNullException(nameof(dbConnection));
    }
    public async Task<int> AddAsync(Tenant tenant)
    {
        string query = @"
                INSERT INTO Tenants
                VALUES (@Id, @UserName, @Password, @Name, @SubscriptionPlan, @Description, @CreatedAt, @IsActive, @Role)";
        using var connection = _dbConnection.CreateConnection();
        int result = await connection.ExecuteAsync(query, new
        {
            tenant.Id,
            tenant.Name,
            tenant.UserName,
            tenant.Password,
            tenant.SubscriptionPlan,
            tenant.Description,
            tenant.CreatedAt,
            tenant.IsActive,
            tenant.Role
        });
        return result;
    }
    public async Task<bool> ExistsAsync(string? userName, Guid? id)
    {
        var connection = _dbConnection.CreateConnection();
        string query = @"";
        bool result;
        if (!string.IsNullOrEmpty(userName))
        {
            query =
                @"SELECT CASE 
                    WHEN EXISTS(SELECT 1 FROM Tenants WHERE UserName = @userName)
                    THEN CAST(1 AS BIT)
                    ELSE CAST(0 AS BIT)
                END";
            result = await connection.ExecuteScalarAsync<bool>(query, new { userName });
            return result;
        }
        query =
            @"SELECT CASE
                WHEN EXISTS(SELECT 1 FROM Tenants WHERE Id = @id)
                THEN CAST(1 AS BIT)
                ELSE CAST(0 AS BIT)
            END";
        result = await connection.ExecuteScalarAsync<bool>(query, new { id });
        return result;
    }
    public async Task<IEnumerable<Tenant>?> GetTenantsAsync(string? searchQuery)
    {
        string query = "";
        using var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = @"SELECT * FROM Tenants
                WHERE CONTAINS ((Name, Description), @searchQuery)";
            var searchResult = await connection.QueryAsync<Tenant>(query, new { searchQuery });
            return searchResult.ToList();
        }
        query =
            @"SELECT
                T.Id,
	            T.Name,
	            T.Description,
	            T.UserName,
	            T.SubscriptionPlan,
	            T.CreatedAt,
	            T.IsActive,
	            T.Role
            FROM
                Tenants T";
        var tenants = await connection.QueryAsync<Tenant>(query);
        return tenants.ToList();
    }
    public async Task<Tenant?> GetTenantAsync(Guid id)
    {
        string query =
            @"SELECT
                Id,
	            Name,
	            Description,
	            UserName,
	            SubscriptionPlan,
	            CreatedAt,
	            IsActive,
	            Role
            FROM Tenants
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var tenant = await connection.QuerySingleOrDefaultAsync<Tenant>(query, new { id });
        return tenant;
    }
    public async Task<Tenant?> GetTenantForAuthAsync(string userName)
    {
        string query =
            @"SELECT
                Id,
	            Name,
	            Description,
	            UserName,
                Password,
	            SubscriptionPlan,
	            CreatedAt,
	            IsActive,
	            Role
            FROM Tenants
            WHERE UserName = @userName";
        var connection = _dbConnection.CreateConnection();
        var tenant = await connection.QuerySingleOrDefaultAsync<Tenant>(query, new { userName });
        return tenant;
    }
    public async Task<Tenant?> GetTenantNameAsync(Guid id)
    {
        string query =
            @"SELECT
                Name
            FROM Tenants
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var name = await connection.QuerySingleOrDefaultAsync<Tenant>(query, new { id });
        return name;
    }
    public async Task<bool> UpdateAsync(Tenant tenant, Guid id)
    {
        var updates = new List<string>();
        if (!string.IsNullOrEmpty(tenant.Name))
        {
            updates.Add("Name = @Name");
        }
        if (!string.IsNullOrEmpty(tenant.Description))
        {
            updates.Add("Description = @Description");
        }
        string query = @$"
                UPDATE Tenants
                SET {string.Join(", ", updates)}
                WHERE Id = @id
                ";
        var connection = _dbConnection.CreateConnection();
        var affected = await connection.ExecuteAsync(query, new
        {
            id,
            tenant.Name,
            tenant.Description
        });
        return affected > 0;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        string query = 
            @"UPDATE Tenants
            SET IsActive = 0
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new { id });
        return result > 0;
    }
}
