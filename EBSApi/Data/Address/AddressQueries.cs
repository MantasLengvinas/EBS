using Dapper;
using EBSApi.Utility;
using EBSApi.Models;
using System.Data;
using EBSApi.Models.Dtos;

namespace EBSApi.Data
{
    public class AddressQueries : IAddressQueries
    {
        private readonly SqlUtility _utility;

        public AddressQueries(SqlUtility utility)
        {
            _utility = utility;
        }

        public async Task<Response<IEnumerable<Address>>> GetAllAddressesAsync()
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Address>> response = new Response<IEnumerable<Address>>();

            parameters.Add(
                "@return_value", 
                dbType: DbType.Int32, 
                direction: ParameterDirection.ReturnValue);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                IEnumerable<Address> addresses = (await connection
                    .QueryAsync<Address>(
                    "dbo.getAllAddresses", 
                    parameters, 
                    commandType: CommandType.StoredProcedure))
                    .ToList();

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };

                }

                response.Data = addresses;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<IEnumerable<Address>> GetAddressesUserAsync(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@userId", userId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {

                IEnumerable<Address> addresses = (await connection
                    .QueryAsync<Address>(
                        "dbo.getAddressesUser",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure))
                    .ToList();

                int returnValue = parameters.Get<int>("@return_value");
                return addresses;
            }
        }

        public async Task<IEnumerable<Address>> GetAddressesProviderAsync(int providerId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@providerId", providerId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {

                IEnumerable<Address> addresses = (await connection
                    .QueryAsync<Address>(
                        "dbo.getAddressesProvider",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure))
                    .ToList();

                int returnValue = parameters.Get<int>("@return_value");
                return addresses;
            }
        }

        public async Task<Address> GetAddressAsync(int addressId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@id", addressId, dbType: DbType.Int32, direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {
                Address address = (await connection
                    .QueryAsync<Address>(
                        "dbo.getAddress",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure))
                    .FirstOrDefault();

                int returnValue = parameters.Get<int>("@return_value");
                return address;
            }
        }
    }
}
