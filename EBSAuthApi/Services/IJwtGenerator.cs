using EBSAuthApi.Models;
using EBSAuthApi.Models.Domain;

namespace EBSAuthApi.Services
{
    public interface IJwtGenerator
    {
        string CreateSessionToken(User user, string audience, string issuer, int expiration);
    }
}