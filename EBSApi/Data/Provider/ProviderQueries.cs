using Dapper;
using EBSApi.Models;
using EBSApi.Utility;
using System.Data;

namespace EBSApi.Data
{
    public class ProviderQueries : IProviderQueries
    {
        private readonly SqlUtility _utility;

        public ProviderQueries(SqlUtility utility)
        {
            _utility = utility;
        }

        public async Task<Provider> GetProviderAsync(int providerId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@id",
                providerId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {
                Provider provider = (await connection
                    .QueryAsync<Provider>
                    ("dbo.getProvider", 
                    parameters, 
                    commandType: CommandType.StoredProcedure))
                    .FirstOrDefault();

                return provider;
            }
        }
        public async Task<IEnumerable<Provider>> GetAllProvidersAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            using (var connection = _utility.CreateConnection())
            {
                IEnumerable<Provider> providers = (await connection
                    .QueryAsync<Provider>
                    ("dbo.getProviders",
                    parameters,
                    commandType: CommandType.StoredProcedure));

                return providers;
            }
        }
    }
}
