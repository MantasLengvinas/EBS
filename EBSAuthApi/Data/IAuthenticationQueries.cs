using EBSAuthApi.Models.Domain;

namespace EBSAuthApi.Data
{
    public interface IAuthenticationQueries
    {
        Task<(int, UserInfo?)> LoginUser(string email, string password, CancellationToken cancelToken = default);
        Task<(int, int)> RegisterUser(string email, string password, string salt, CancellationToken cancelToken = default);
    }
}