using System.Data;
using System.Data.SqlClient;

namespace EBSApi.Utility
{
    public class SqlUtility
    {
        private readonly string _connectionString;

        public SqlUtility(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}