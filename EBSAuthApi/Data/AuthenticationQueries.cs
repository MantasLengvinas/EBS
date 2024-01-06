using System;
using System.Data;
using Dapper;
using EBSAuthApi.Helpers;
using EBSAuthApi.Models.Domain;
using EBSAuthApi.Models.Dtos.Requests;
using EBSAuthApi.Utility;

namespace EBSAuthApi.Data
{
    public class AuthenticationQueries : IAuthenticationQueries
    {
        private readonly SqlUtility _sqlUtility;

        public AuthenticationQueries(SqlUtility sqlUtility)
        {
            _sqlUtility = sqlUtility ?? throw new ArgumentNullException(nameof(sqlUtility));
        }

        public async Task<(int, string?)> GetPassword(string email, CancellationToken cancelToken)
        {
            DynamicParameters parameters = new();

            parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@email", email, dbType: DbType.String, direction: ParameterDirection.Input);

            using (IDbConnection connection = _sqlUtility.CreateConnection())
            {
                string? password = (await connection.
                    QueryAsync<string>("AUTH.getPassword",
                                         parameters,
                                         commandType: CommandType.StoredProcedure)).FirstOrDefault();

                int returnValue = parameters.Get<int>("@return_value");

                return (returnValue, password);
            }
        }

        public async Task<(int, UserInfo?)> LoginUser(string email, CancellationToken cancelToken)
        {
            DynamicParameters parameters = new();

            parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@email", email, dbType: DbType.String, direction: ParameterDirection.Input);

            using (IDbConnection connection = _sqlUtility.CreateConnection())
            {
                UserInfo? user = (await connection.
                    QueryAsync<UserInfo>("AUTH.login",
                                         parameters,
                                         commandType: CommandType.StoredProcedure)).FirstOrDefault();

                int returnValue = parameters.Get<int>("@return_value");

                return (returnValue, user);
            }
        }

        public async Task<(int, int)> RegisterUser(string email, string password, CancellationToken cancelToken)
        {
            DynamicParameters parameters = new();

            parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@id", dbType: DbType.Int32, direction: ParameterDirection.Output);
            parameters.Add("@email", email, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@password", password, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@action", "create", dbType: DbType.String, direction: ParameterDirection.Input);


            using (IDbConnection connection = _sqlUtility.CreateConnection())
            {
                await connection
                    .QueryAsync("AUTH.registerClient",
                                parameters,
                                commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@return_value");
                int id = parameters.Get<int>("@id");

                return (returnValue, id);
            }
        }

        public async Task<int> CompleteRegistration(CompleteRegistration userInfo, CancellationToken cancelToken)
        {
            DynamicParameters parameters = new();

            parameters.Add("@return_value", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
            parameters.Add("@id", userInfo.Id, dbType: DbType.Int32, direction: ParameterDirection.InputOutput);
            parameters.Add("@fullName", userInfo.FullName, dbType: DbType.String, direction: ParameterDirection.Input);
            parameters.Add("@business", userInfo.Business, dbType: DbType.Boolean, direction: ParameterDirection.Input);
            parameters.Add("@balance", userInfo.Balance, dbType: DbType.Decimal, direction: ParameterDirection.Input);
            parameters.Add("@action", "complete", dbType: DbType.String, direction: ParameterDirection.Input);


            using (IDbConnection connection = _sqlUtility.CreateConnection())
            {
                await connection
                    .QueryAsync("AUTH.registerClient",
                                parameters,
                                commandType: CommandType.StoredProcedure);

                int returnValue = parameters.Get<int>("@return_value");

                return returnValue;
            }
        }
    }
}

