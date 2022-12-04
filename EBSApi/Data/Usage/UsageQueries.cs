using Dapper;
using EBSApi.Models;
using EBSApi.Models.Dtos;
using EBSApi.Utility;
using System.Data;

namespace EBSApi.Data
{
    public class UsageQueries : IUsageQueries
    {
        private readonly SqlUtility _utility;

        public UsageQueries(SqlUtility utility)
        {
            _utility = utility;
        }

        public async Task<Response<Usage>> GetUsageAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<Usage> response = new Response<Usage>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@id", id,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                Usage usage = (await connection
                    .QueryAsync<Usage>(
                        "dbo.getUsage",
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

                response.Data = usage;
                response.IsSuccess = true;
                return response;
            }
        }
        public async Task<Response<IEnumerable<Usage>>> GetUserUsagesForMonthAsync(int year, int month, int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Usage>> response = new Response<IEnumerable<Usage>>();

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
                "@userId", userId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                IEnumerable<Usage> usage = (await connection
                    .QueryAsync<Usage>(
                        "dbo.getUserUsageForMonth",
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

                response.Data = usage;
                response.IsSuccess = true;
                return response;
            }
        }
        public async Task<Response<IEnumerable<Usage>>> GetAddressUsagesForMonthAsync(int year, int month, int addressId)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Usage>> response = new Response<IEnumerable<Usage>>();

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
                "@addressId", addressId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                IEnumerable<Usage> usage = (await connection
                    .QueryAsync<Usage>(
                        "dbo.getAddressUsageForMonth",
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

                response.Data = usage;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<IEnumerable<Usage>>> GetAllUsagesAsync()
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<Usage>> response = new Response<IEnumerable<Usage>>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                IEnumerable<Usage> usage = (await connection
                    .QueryAsync<Usage>(
                        "dbo.getUsages",
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

                response.Data = usage;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<PaymentDto>> GetAddressUnpaidUsagesAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<PaymentDto> response = new Response<PaymentDto>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@id", id,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                SqlMapper.GridReader res = await connection
                    .QueryMultipleAsync(
                        "dbo.getAddressUnpaidUsages",
                        parameters,
                        commandType: CommandType.StoredProcedure);

                response.Data = new PaymentDto
                {
                    Usages = await res.ReadAsync<Usage>(),
                    PaymentSum = (await res.ReadAsync<double>()).FirstOrDefault()
                };

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

                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<Usage>> SetUsagePaidAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<Usage> response = new Response<Usage>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);
            
            parameters.Add(
                "@id", id,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                Usage res = (await connection
                    .QueryAsync<Usage>(
                        "dbo.setUsagePaid",
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

                response.Data = res;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<Usage>> CreateUsageAsync(Usage usage)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<Usage> response = new Response<Usage>();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@onDate", usage.OnDate,
                dbType: DbType.Date,
                direction: ParameterDirection.Input);

            parameters.Add(
                "@electricityDue", usage.ElectricityDue,
                dbType: DbType.Double,
                direction: ParameterDirection.Input);
            
            parameters.Add(
                "@isPaid", usage.IsPaid,
                dbType: DbType.Boolean,
                direction: ParameterDirection.Input);            
            
            parameters.Add(
                "@addressId", usage.AddressId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);        
            
            parameters.Add(
                "@paidTariff", usage.PaidTariff,
                dbType: DbType.Double,
                direction: ParameterDirection.Input);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                (usage.UsageId, usage.AmountDue, usage.PaidTariff) = (await connection
                    .QueryAsync<(int, double, double)>(
                        "dbo.createUsage",
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

                response.Data = usage;
                response.IsSuccess = true;
                return response;
            }
        }
    }
}
