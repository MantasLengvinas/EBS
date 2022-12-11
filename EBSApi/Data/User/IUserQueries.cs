using EBSApi.Models;
using EBSApi.Models.Dtos;

namespace EBSApi.Data
{
    public interface IUserQueries
    {
        public Task<Response<IEnumerable<User>>> GetAllUsersAsync();
        public Task<Response<User>> GetUserAsync(int id);
        Task<int> DeleteUserAsync(int id);
        public Task<Response<User>> CreateUserAsync(User user);
    }
}
