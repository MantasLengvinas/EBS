using EBSAuthApi.Models;
using EBSAuthApi.Models.Dtos.Requests;
using EBSAuthApi.Models.Dtos.Responses;

namespace EBSAuthApi.Services
{
    public interface IAuthenticationService
    {
        Task<AuthResponseDto> LoginUserAsync(UserLogin userLogin, CancellationToken cancelToken = default);
        Task<AuthResponseDto> RegisterClientAsync(ClientRegister userLogin, CancellationToken cancelToken = default);
        Task<bool> CompleteRegistrationAsync(CompleteRegistration userInfo, CancellationToken cancelToken = default);
    }
}