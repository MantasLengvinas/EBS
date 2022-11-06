using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EBSAuthenticationHandler.Services
{
    public interface IUserAuthService
    {
        Task<AuthenticateResult> LoginUser(object userCredentials);
        Task<AuthenticateResult> RegisterClient(object userCredentials);
        Task<bool> CompleteUserRegistration(object userInfo);

    }
}