using EBSApi.Models;

namespace EBSApi.Data
{
    public interface IUserQueries
    {
        public Task<IEnumerable<User>> GetAllUsersAsync();
        public Task<User> GetUserAsync(int id);
    }
}
