using Dapper;
using EBSApi.Utility;
using EBSApi.Models;
using System.Data;

namespace EBSApi.Data
{
    public class UserQueries : IUserQueries
    {
        private readonly SqlUtility _utility;

        public UserQueries(SqlUtility utility)
        {
            _utility = utility;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add(
                "@return_value", 
                dbType: DbType.Int32, 
                direction: ParameterDirection.ReturnValue);

            using (var connection = _utility.CreateConnection())
            {
                IEnumerable<User> users = await connection.QueryAsync<User>("dbo.getUsers", parameters, commandType: System.Data.CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@return_value");
                return users;
            }
        }

        public async Task<User> GetUserAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();

            parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@id", id, dbType: DbType.Int32, direction: ParameterDirection.Input);

            using (var connection = _utility.CreateConnection())
            {

                User user = (await connection
                    .QueryAsync<User>(
                        "dbo.getUser",
                        parameters,
                        commandType: System.Data.CommandType.StoredProcedure))
                    .FirstOrDefault();

                int returnValue = parameters.Get<int>("@return_value");
                return user;
            }
        }

    }
}
