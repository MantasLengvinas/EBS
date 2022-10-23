using Dapper;
using EBSApi.Utility;
using EBSApi.Models;

namespace EBSApi.Services
{
    public class UserService : IUserService
    {
        private readonly SqlUtility _utility;

        public UserService(SqlUtility utility)
        {
            _utility = utility;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using (var connection = _utility.CreateConnection())
            {
                IEnumerable<User> users = await connection.QueryAsync<User>("dbo.getUsers", commandType: System.Data.CommandType.StoredProcedure);
                return users;
            }
        }

        public async Task<User> GetUser(int id)
        {
            using (var connection = _utility.CreateConnection())
            {
                User user = (await connection.QueryAsync<User>("dbo.getUser", new { id }, commandType: System.Data.CommandType.StoredProcedure)).FirstOrDefault();
                return user;
            }
        }

    }
}
