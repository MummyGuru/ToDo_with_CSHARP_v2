using System.Data;
using System.Data.SqlClient;

namespace ToDo_with_CSHARP_v2.EntityCore.DataAccess
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }

    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(string connectionString = null)
        {
            _connectionString = connectionString ?? DatabaseConfig.ConnectionString;
        }

        public IDbConnection CreateConnection()
        {
            var connection = new SqlConnection(_connectionString);
            connection.Open();
            return connection;
        }
    }
}
