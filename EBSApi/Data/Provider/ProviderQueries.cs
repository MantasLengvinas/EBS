using Dapper;
using EBSApi.Models;
using EBSApi.Models.Dtos;
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

        public async Task<Response<Provider>> GetProviderAsync(int providerId)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<Provider> response = new Response<Provider>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@id", providerId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                Provider provider = (await connection
                    .QueryAsync<Provider>(
                        "dbo.getProvider", 
                        parameters, 
                        commandType: CommandType.StoredProcedure))
                    .FirstOrDefault();

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                    return response;
                }

                response.Data = provider;
                response.IsSuccess = true;
                return response;
            }
        }
        public async Task<Response<IEnumerable<Provider>>> GetAllProvidersAsync()
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Provider>> response = new Response<IEnumerable<Provider>>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            using (var connection = _utility.CreateConnection())
            {
                IEnumerable<Provider> providers = (await connection
                    .QueryAsync<Provider>(
                        "dbo.getProviders",
                        parameters,
                        commandType: CommandType.StoredProcedure));

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                    return response;
                }

                response.Data = providers;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<Provider>> CreateProviderAsync(Provider provider)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<Provider> response = new Response<Provider>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue); 
            
            parameters.Add(
                 "@providerName", provider.ProviderName,
                 dbType: DbType.String,
                 direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {
                provider.ProviderId = (await connection
                    .QueryAsync<int>(
                        "dbo.createProvider",
                        parameters,
                        commandType: CommandType.StoredProcedure))
                        .FirstOrDefault();

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                    return response;
                }

                response.Data = provider;
                response.IsSuccess = true;
                return response;
            }
        }
    }
}
