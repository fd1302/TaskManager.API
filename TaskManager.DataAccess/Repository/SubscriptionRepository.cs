using Dapper;
using TaskManager.DataAccess.DbConnection;
using TaskManager.Shared.Dto_s.Subscription;
using TaskManager.Shared.Entities;

namespace TaskManager.DataAccess.Repository;

public class SubscriptionRepository
{
    private readonly DataBaseConnection _dbConnection;
    public SubscriptionRepository(DataBaseConnection dbConnection)
    {
        _dbConnection = dbConnection ??
            throw new ArgumentNullException(nameof(dbConnection));
    }
    public async Task<bool> AddAsync(Subscription subscription)
    {
        // Check if a subplan already exists with tenantid
        int count = await CheckSubscriptionCountAsync(subscription.TenantId);
        if (count == 1 || count > 1)
        {
            throw new ArgumentException("Tenant already has sub plan try updating the existing one.");
        }
        string query =
            @"INSERT INTO Subscriptions
            VALUES (@Id, @TenantId, @TenantName, @StartDate, @EndDate, @Duration, @Expired)";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new
        {
            subscription.Id,
            subscription.TenantId,
            subscription.TenantName,
            subscription.StartDate,
            subscription.EndDate,
            subscription.Duration,
            subscription.Expired
        });
        return result > 0;
    }
    public async Task<int> CheckSubscriptionCountAsync(Guid tenantId)
    {
        string query =
            @"SELECT
                COUNT(TenantId)
            FROM Subscriptions
            WHERE TenantId = @tenantId
            ";
        var connection = _dbConnection.CreateConnection();
        int count = await connection.ExecuteScalarAsync<int>(query, new { tenantId });
        return count;
    }
    public async Task<IEnumerable<Subscription>?> GetSubscriptionsAsync()
    {
        string query =
            @"SELECT
                Id,
                TenantName,
                TenantId,
                StartDate,
                EndDate,
                Duration
            FROM Subscriptions";
        var connection = _dbConnection.CreateConnection();
        var subPlans = await connection.QueryAsync<Subscription>(query);
        return subPlans;
    }
    public async Task<Subscription?> GetSubscriptionAsync(Guid? id, Guid? tenantId)
    {
        string query = @"";
        var connection = _dbConnection.CreateConnection();
        if (id != null)
        {
            query =
                @"SELECT
                    Id,
                    TenantId,
                    TenantName,
                    StartDate,
                    EndDate,
                    Duration
                FROM Subscriptions
                WHERE Id = @id";
            var withId = await connection.QueryFirstOrDefaultAsync<Subscription>(query, new { id });
            return withId;
        }
        else if (tenantId != null)
        {
            query =
                @"SELECT
                    Id,
                    TenantId,
                    TenantName,
                    StartDate,
                    EndDate,
                    Duration
                FROM Subscriptions
                WHERE TenantId = @tenantId";
            var withTenantId = await connection.QuerySingleOrDefaultAsync<Subscription>(query, new { tenantId });
            return withTenantId;
        }
        return null;
    }
    public async Task<bool> UpdateSubscriptionAsync(Guid id, Subscription subscription)
    {
        string query =
            @"UPDATE Subscriptions
            SET StartDate = @StartDate, EndDate = @EndDate, Duration = @Duration
            WHERE TenantId = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new
        {
            id,
            subscription.StartDate,
            subscription.EndDate,
            subscription.Duration
        });
        return result > 0;
    }
    public async Task<bool> DeleteSubscriptionAsync(Guid id)
    {
        string query =
            @"DELETE Subscriptions
            WHERE TenantId = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new { id });
        return result > 0;
    }
    public async Task<bool> AddSubscriptionHistoryAsync(SubscriptionHistory subscriptionHistory)
    {
        string query =
            @"INSERT INTO SubscriptionHistories
            VALUES (@Id, @TenantId, @TenantName, @StartDate, @EndDate, @Duration)";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteAsync(query, new
        {
            subscriptionHistory.Id,
            subscriptionHistory.TenantId,
            subscriptionHistory.TenantName,
            subscriptionHistory.StartDate,
            subscriptionHistory.EndDate,
            subscriptionHistory.Duration
        });
        return result > 0;
    }
    public async Task<IEnumerable<SubscriptionHistory>?> GetSubscriptionHistoriesAsync(string? searchQuery)
    {
        string query = @"";
        var connection = _dbConnection.CreateConnection();
        if (!string.IsNullOrEmpty(searchQuery))
        {
            query =
                @"SELECT
                    Id,
                    TenantId,
                    TenantName,
                    StartDate,
                    EndDate,
                    Duration
                FROM SubscriptionHistories
                WHERE CONTAINS((TenantName), @searchQuery)";
            var searchResult = await connection.QueryAsync<SubscriptionHistory>(query, new { searchQuery });
            return searchResult;
        }
        query =
            @"SELECT
                Id,
                TenantId,
                TenantName,
                StartDate,
                EndDate,
                Duration
            FROM SubscriptionHistories";
        var result = await connection.QueryAsync<SubscriptionHistory>(query);
        return result;
    }
    public async Task<IEnumerable<SubscriptionHistory>?> GetSubscriptionHistoriesWithTenantIdAsync(Guid id)
    {
        string query =
            @"SELECT
                SH.Id,
                SH.TenantId,
                SH.TenantName,
                SH.StartDate,
                SH.EndDate,
                SH.Duration
            FROM SubscriptionHistories SH
            JOIN Tenants T ON T.Id = SH.TenantId
            WHERE T.Id = @id";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.QueryAsync<SubscriptionHistory>(query, new { id });
        return result;
    }
    public async Task<bool> SubPlanExistsAsync(Guid id)
    {
        string query =
            @"SELECT CASE
                WHEN EXISTS (SELECT 1 FROM Subscriptions WHERE TenantId = @id)
                THEN CAST (1 AS BIT)
                ELSE CAST (0 AS BIT)
            END";
        var connection = _dbConnection.CreateConnection();
        var result = await connection.ExecuteScalarAsync<bool>(query, new { id });
        return result;
    }
}
