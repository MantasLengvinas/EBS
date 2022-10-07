using EBSAuthApi.Models;
using EBSAuthApi.Models.Dtos.Requests;
using EBSAuthApi.Models.Dtos.Responses;

namespace EBSAuthApi.Services
{
    public interface IAuthenticationService
    {
        Task<string> AuthenticateUser(UserLogin userLogin, CancellationToken cancelToken);
    }
}