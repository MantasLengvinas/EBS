using EBSApi.Models;

namespace EBSApi.Repositories
{
    public interface IUserRepository
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetUser(int id);
    }
}
