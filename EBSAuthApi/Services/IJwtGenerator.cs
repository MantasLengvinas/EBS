using EBSAuthApi.Models;

namespace EBSAuthApi.Services
{
    public interface IJwtGenerator
    {
        string CreateAccessToken(User user, string audience, string issuer, int expiration);
        string CreateIdToken(string accessToken, string email, string audience, string issuer, int expiration);
        string CreateRefreshToken(string accessToken, string audience, string issuer, int expiration);
    }
}