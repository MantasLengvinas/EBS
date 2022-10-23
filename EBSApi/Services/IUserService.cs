using EBSApi.Models;

namespace EBSApi.Services
{
    public interface IUserService
    {
        public Task<IEnumerable<User>> GetAllUsers();
        public Task<User> GetUser(int id);
    }
}
