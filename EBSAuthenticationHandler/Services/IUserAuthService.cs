using Microsoft.AspNetCore.Authentication;

namespace EBSAuthenticationHandler.Services
{
    public interface IUserAuthService
    {
        Task<AuthenticateResult> LoginUser(object userCredentials);
    }
}