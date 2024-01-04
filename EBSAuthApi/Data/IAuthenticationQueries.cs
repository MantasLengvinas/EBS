using EBSAuthApi.Models.Domain;
using EBSAuthApi.Models.Dtos.Requests;

namespace EBSAuthApi.Data
{
    public interface IAuthenticationQueries
    {
        Task<(int, string?)> GetPassword(string email, CancellationToken cancelToken = default);
        Task<(int, UserInfo?)> LoginUser(string email, CancellationToken cancelToken = default);
        Task<(int, int)> RegisterUser(string email, string password, CancellationToken cancelToken = default);
        Task<int> CompleteRegistration(CompleteRegistration userInfo, CancellationToken cancelToken = default);
    }
}