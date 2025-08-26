using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace TaskManager.DataAccess.DbConnection;

public class DataBaseConnection
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionString;
    public DataBaseConnection(IConfiguration configuration)
    {
        _configuration = configuration ??
            throw new ArgumentNullException(nameof(configuration));
        _connectionString = _configuration.GetConnectionString("DefaultString")!;
    }
    public IDbConnection CreateConnection() => new SqlConnection(_connectionString);
}
