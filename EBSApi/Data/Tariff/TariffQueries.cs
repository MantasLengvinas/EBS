using Dapper;
using EBSApi.Models;
using EBSApi.Models.Dtos;
using EBSApi.Utility;
using System.Data;

namespace EBSApi.Data
{
    public class TariffQueries : ITariffQueries
    {
        private readonly SqlUtility _utility;

        public TariffQueries(SqlUtility utility)
        {
            _utility = utility;
        }
        public async Task<Response<Tariff>> GetTariffByIdAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<Tariff> response = new Response<Tariff>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);
            parameters.Add(
                "@tariffId", id,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {

                Tariff tariff = (await connection
                    .QueryAsync<Tariff>(
                        "dbo.getTariff",
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
                }

                response.Data = tariff;
                response.IsSuccess = true;
                return response;
            }
        }
        public async Task<Response<IEnumerable<Tariff>>> GetLatestTariffsByMonthAsync(int year, int month, int providerId)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Tariff>> response = new Response<IEnumerable<Tariff>>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);
            parameters.Add(
                "@year", year,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);
            parameters.Add(
                "@month", month,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);
            parameters.Add(
                "@providerId", providerId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {

                IEnumerable<Tariff> tariffs = (await connection
                    .QueryAsync<Tariff>(
                        "dbo.getLatestTariff",
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

                response.Data = tariffs;
                response.IsSuccess = true;
                return response;
            }
        }
    }
}
