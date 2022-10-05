using EBSApi.Models.Authentication;

namespace EBSApi.Services.Authentication
{
    public interface IAuthenticationService
    {
        string GetJwt(User user, CancellationToken cancelToken = default);
    }
}