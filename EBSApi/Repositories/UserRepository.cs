using Dapper;
using EBSApi.Context;
using EBSApi.Models;

namespace EBSApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContext _context;

        public UserRepository(DbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            using (var connection = _context.CreateConnection())
            {
                IEnumerable<User> users = await connection.QueryAsync<User>("dbo.getUsers", commandType: System.Data.CommandType.StoredProcedure);
                return users;
            }
        }

        public async Task<User> GetUser(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                User user = (await connection.QueryAsync<User>("dbo.getUser", new { id }, commandType: System.Data.CommandType.StoredProcedure)).First();
                return user;
            }
        }

    }
}
