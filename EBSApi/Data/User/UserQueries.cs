using Dapper;
using EBSApi.Utility;
using EBSApi.Models;
using System.Data;
using EBSApi.Models.Dtos;

namespace EBSApi.Data
{
    public class UserQueries : IUserQueries
    {
        private readonly SqlUtility _utility;

        public UserQueries(SqlUtility utility)
        {
            _utility = utility;
        }

        public async Task<Response<IEnumerable<User>>> GetAllUsersAsync()
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<IEnumerable<User>> response = new Response<IEnumerable<User>>();

            parameters.Add(
                "@return_value", 
                dbType: DbType.Int32, 
                direction: ParameterDirection.ReturnValue);

            using (IDbConnection connection = _utility.CreateConnection())
            {
                IEnumerable<User> users = await connection
                    .QueryAsync<User>(
                        "dbo.getUsers", 
                        parameters, 
                        commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@return_value");

                if (returnValue != 0)
                {
                    response.Error = new Error
                    {
                        ErrorMessage = $"SQL exception occured with the return value of {returnValue}",
                        StatusCode = 400
                    };
                }

                response.Data = users;
                response.IsSuccess = true;
                return response;
            }
        }

        public async Task<Response<User>> GetUserAsync(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            Response<User> response = new Response<User>();

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

                User user = (await connection
                    .QueryAsync<User>(
                        "dbo.getUser",
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

                response.Data = user;
                response.IsSuccess = true;
                return response;
            }
        }

    }
}
