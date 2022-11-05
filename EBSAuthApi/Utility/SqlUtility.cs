using System;
using System.Data;
using System.Data.SqlClient;

namespace EBSAuthApi.Utility
{
    public class SqlUtility
    {
        private readonly string _connectionString;

        public SqlUtility(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}

