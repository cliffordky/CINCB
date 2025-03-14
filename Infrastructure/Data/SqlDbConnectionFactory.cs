using Core;
using Microsoft.Extensions.Configuration;
using System.Data.Common;

namespace Infrastructure.Data
{
    /// <summary>
    /// This class provides methods to create <see cref="DbConnection" /> instances of a concrete type.
    /// </summary>
    /// <typeparam name="TDbConnection">The concrete <see cref="DbConnection" /> implementation to instantiate.</typeparam>
    public class SqlDbConnectionFactory<TDbConnection> : ISqlConnectionFactory where TDbConnection : DbConnection, new()
    {
        private const string DEFAULT_CONNECTION = "DefaultConnection";

        private readonly string _connectionString;

        public Type DbConnectionType => typeof(TDbConnection);

        public string ConnectionString => _connectionString;

        public SqlDbConnectionFactory(IConfiguration configuration)
        {
            Argument.NotNull(configuration, nameof(configuration));

            _connectionString = Argument.NotNull(configuration.GetConnectionString(DEFAULT_CONNECTION), "ConnectionString", "No connection string configured. Check appsettings.json");
        }

        public DbConnection CreateConnection()
        {
            var connection = new TDbConnection
            {
                ConnectionString = _connectionString
            };

            connection.Open();

            return connection;
        }
    }
}
