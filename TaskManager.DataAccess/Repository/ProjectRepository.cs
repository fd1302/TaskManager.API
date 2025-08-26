using Dapper;
using TaskManager.DataAccess.DbConnection;
using TaskManager.Shared.Dto_s.Project;
using TaskManager.Shared.Entities;

namespace TaskManager.DataAccess.Repository;

public class ProjectRepository
{
    private readonly DataBaseConnection _dbConnection;
    public ProjectRepository(DataBaseConnection dbConnection)
    {
        _dbConnection = dbConnection ??
            throw new ArgumentNullException(nameof(dbConnection));
    }
    public async Task<int> AddAsync(Project project)
    {
        string query =
            @"INSERT INTO Projects
            VALUES (@Id, @TenantId, @Name, @Description, @CreatedAt)";
        var connection = _dbConnection.CreateConnection();
        int result = await connection.ExecuteAsync(query, new
        {
            project.Id,
            project.TenantId,
            project.Name,
            project.Description,
            project.CreatedAt
        });
        return result;
    }
    public async Task<IEnumerable<Project>?> GetProjectsAsync(string? searchQuery)
    {
        string query = @"";
        var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query =
                @"SELECT
                    P.Id,
                    P.TenantId,
                    P.Name,
                    P.Description
                    P.CreatedAt
                FROM
                    Projects P
                WHERE CONTAINS((Name, Description), @searchQuery)";
            var searchResult = await connection.QueryAsync<Project>(query, new { searchQuery });
            return searchResult;
        }
        query = @"SELECT * FROM Projects";
        var projects = await connection.QueryAsync<Project>(query);
        return projects;
    }
    public async Task<Project?> GetProjectAsync(Guid id)
    {
        string query =
            @"SELECT
                    P.Id,
                    P.TenantId,
                    P.Name,
                    P.Description,
                    P.CreatedAt
            FROM
                Projects P
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QuerySingleOrDefaultAsync<Project?>(query, new { id });
        return result;
    }
    public async Task<IEnumerable<Project>?> GetProjectsWithTenantIdAsync(Guid id)
    {
        string query =
            @"SELECT
                P.Id,
                P.TenantId,
                P.Name,
                P.Description,
                P.CreatedAt
            FROM Projects P
            LEFT JOIN Tenants T ON T.Id = P.TenantId
            WHERE P.TenantId = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QueryAsync<Project>(query, new { id });
        return result;
    }
    public async Task<bool> UpdateAsync(Guid id, Project project)
    {
        var updates = new List<string>();
        if (!string.IsNullOrEmpty(project.Name))
        {
            updates.Add("Name = @Name");
        }
        if (!string.IsNullOrEmpty(project.Description))
        {
            updates.Add("Description = @Description");
        }
        string query =
            $@"
            UPDATE Projects
            SET {string.Join(", ", updates)}
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new
        {
            id,
            project.Name,
            project.Description
        });
        return result > 0;
    }
    public async Task<bool> DeleteAsync(Guid id)
    {
        string query =
            @"
            DELETE FROM Projects
            WHERE Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new { id });
        return result > 0;
    }
    public async Task<bool> ProjectExistsAsync(Guid id)
    {
        string query =
            @"SELECT CASE
                WHEN EXISTS(SELECT 1 FROM Projects WHERE Id = @id)
                THEN CAST(1 AS BIT)
                ELSE CAST(0 AS BIT)
            END";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteScalarAsync<bool>(query, new { id });
        return result;
    }
}
