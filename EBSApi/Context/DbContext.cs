using System.Data;
using System.Data.SqlClient;

namespace EBSApi.Context
{
    public class DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public DbContext(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        public IDbConnection CreateConnection()
            => new SqlConnection(_connectionString);
    }
}
