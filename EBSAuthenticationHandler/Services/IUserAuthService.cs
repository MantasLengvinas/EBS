using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;

namespace EBSAuthenticationHandler.Services
{
    public interface IUserAuthService
    {
        Task<AuthenticateResult> LoginUser(object userCredentials);
    }
}