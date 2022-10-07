namespace EBSAuthenticationHandler.Services
{
    internal interface ITokenRefreshService
    {
        Task<string> RefreshAccessToken(string refreshToken);
    }
}