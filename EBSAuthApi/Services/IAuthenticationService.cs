using EBSAuthApi.Models;
using EBSAuthApi.Models.Dtos.Requests;
using EBSAuthApi.Models.Dtos.Responses;

namespace EBSAuthApi.Services
{
    public interface IAuthenticationService
    {
        //Task<AuthResponseDto> LoginUserAsync(UserLoginDto userLogin, CancellationToken cancelToken = default);
        //Task<AuthResponseDto> RegisterClientAsync(UserRegistrationDto userLogin, CancellationToken cancelToken = default);
        Task<AuthResponseDto> LoginUserAsync(IUserCredentials credentials, CancellationToken cancelToken = default);
        Task<AuthResponseDto> RegisterUserAsync(IUserCredentials credentials, CancellationToken cancelToken = default);
        Task<bool> CompleteRegistrationAsync(CompleteRegistration userInfo, CancellationToken cancelToken = default);
    }
}