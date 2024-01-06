/*
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
   
   using IDbConnection connection = _utility.CreateConnection();
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
   ErrorMessage = $"SQL exception occurred with the return value of {returnValue}",
   StatusCode = 400
   };
   return response;
   }
   
   response.Data = tariff;
   response.IsSuccess = true;
   return response;
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
   
   using IDbConnection connection = _utility.CreateConnection();
   
   IEnumerable<Tariff> tariffs = await connection
   .QueryAsync<Tariff>(
   "dbo.getLatestTariff",
   parameters,
   commandType: CommandType.StoredProcedure);
   
   int returnValue = parameters.Get<int>("@return_value");
   
   if (returnValue != 0)
   {
   response.Error = new Error
   {
   ErrorMessage = $"SQL exception occurred with the return value of {returnValue}",
   StatusCode = 400
   };
   return response;
   }
   
   response.Data = tariffs;
   response.IsSuccess = true;
   return response;
   }
   
   public async Task<Response<IEnumerable<Tariff>>> GetHistoricalTariffDataAsync(int providerId, bool isBusiness)
   {
   DynamicParameters parameters = new DynamicParameters();
   Response<IEnumerable<Tariff>> response = new Response<IEnumerable<Tariff>>();
   
   parameters.Add(
   "@return_value",
   dbType: DbType.Int32,
   direction: ParameterDirection.ReturnValue);
   parameters.Add(
   "@providerId", providerId,
   dbType: DbType.Int32,
   direction: ParameterDirection.Input);
   parameters.Add(
   "@isBusiness", isBusiness,
   dbType: DbType.Int32,
   direction: ParameterDirection.Input);
   
   using IDbConnection connection = _utility.CreateConnection();
   
   IEnumerable<Tariff> tariffs = await connection
   .QueryAsync<Tariff>(
   "dbo.getHistoricTariffData",
   parameters,
   commandType: CommandType.StoredProcedure);
   
   int returnValue = parameters.Get<int>("@return_value");
   
   if (returnValue != 0)
   {
   response.Error = new Error
   {
   ErrorMessage = $"SQL exception occurred with the return value of {returnValue}",
   StatusCode = 400
   };
   return response;
   }
   
   response.Data = tariffs;
   response.IsSuccess = true;
   return response;
   }
   }
   }
 */

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
            return await ExecuteStoredProcedureSingleAsync<Tariff>(
                "dbo.getTariff",
                new
                {
                    tariffId = id
                }
            );
        }

        public async Task<Response<IEnumerable<Tariff>>> GetLatestTariffsByMonthAsync(int year, int month, int providerId)
        {
            return await ExecuteStoredProcedureAsync<Tariff>(
                "dbo.getLatestTariff",
                new
                {
                    year,
                    month,
                    providerId
                }
            );
        }

        public async Task<Response<IEnumerable<Tariff>>> GetHistoricalTariffDataAsync(int providerId, bool isBusiness)
        {
            return await ExecuteStoredProcedureAsync<Tariff>(
                "dbo.getHistoricTariffData",
                new
                {
                    providerId,
                    isBusiness
                }
            );
        }

        private async Task<Response<T>> ExecuteStoredProcedureSingleAsync<T>(string storedProcedure, object parameters)
        {
            var response = new Response<T>();

            using IDbConnection connection = _utility.CreateConnection();
            var result = await ExecuteStoredProcedureAsync<T>(storedProcedure, parameters);
            response.Data = result.Data.SingleOrDefault();
            response.IsSuccess = result.IsSuccess;
            response.Error = result.Error;

            return response;
        }

        private async Task<Response<IEnumerable<T>>> ExecuteStoredProcedureAsync<T>(string storedProcedure, object parameters)
        {
            var response = new Response<IEnumerable<T>>();
            var dbParameters = new DynamicParameters(parameters);

            dbParameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            using IDbConnection connection = _utility.CreateConnection();
            var data = await connection.QueryAsync<T>(
                storedProcedure,
                dbParameters,
                commandType: CommandType.StoredProcedure);

            int returnValue = dbParameters.Get<int>("@return_value");
            if (returnValue != 0)
            {
                response.Error = new Error
                {
                    ErrorMessage = $"SQL exception occurred with the return value of {returnValue}",
                    StatusCode = 400
                };
            }
            else
            {
                response.Data = data;
                response.IsSuccess = true;
            }
            return response;
        }
    }
}
