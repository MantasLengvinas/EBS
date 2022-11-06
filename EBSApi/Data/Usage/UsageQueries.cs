using Dapper;
using EBSApi.Models;
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

        public async Task<Usage> GetUsageAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@id",
                id,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {
                Usage usage = (await connection
                    .QueryAsync<Usage>
                    ("dbo.getUsage",
                    parameters,
                    commandType: CommandType.StoredProcedure))
                    .FirstOrDefault();

                return usage;
            }
        }
        public async Task<IEnumerable<Usage>> GetUserUsagesForMonthAsync(int year, int month, int userId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@year",
                year,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            parameters.Add(
                "@month",
                month,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            parameters.Add(
                "@userId",
                userId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {
                IEnumerable<Usage> usage = (await connection
                    .QueryAsync<Usage>
                    ("dbo.getUserUsageForMonth",
                    parameters,
                    commandType: CommandType.StoredProcedure));

                return usage;
            }
        }
        public async Task<IEnumerable<Usage>> GetAddressUsagesForMonthAsync(int year, int month, int addressId)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            parameters.Add(
                "@year",
                year,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            parameters.Add(
                "@month",
                month,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            parameters.Add(
                "@addressId",
                addressId,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {
                IEnumerable<Usage> usage = (await connection
                    .QueryAsync<Usage>
                    ("dbo.getAddressUsageForMonth",
                    parameters,
                    commandType: CommandType.StoredProcedure));

                return usage;
            }
        }

        public async Task<IEnumerable<Usage>> GetAllUsagesAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);

            using (var connection = _utility.CreateConnection())
            {
                IEnumerable<Usage> usage = (await connection
                    .QueryAsync<Usage>
                    ("dbo.getUsages",
                    parameters,
                    commandType: CommandType.StoredProcedure));

                return usage;
            }
        }

        public async Task<int> SetUsagePaid(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                "@return_value",
                dbType: DbType.Int32,
                direction: ParameterDirection.ReturnValue);
            
            parameters.Add(
                "@id",
                id,
                dbType: DbType.Int32,
                direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {
                await connection
                    .QueryAsync<Usage>
                    ("dbo.setUsagePaid",
                    parameters,
                    commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@return_value");
                return returnValue;
            }
        }
    }
}
