using System;
using EBSAuthApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EBSAuthApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using EBSAuthApi.Models.Dtos.Requests;
using EBSAuthApi.Models.Dtos.Responses;
using EBSAuthApi.Models.Domain;
using EBSAuthApi.Constants;

namespace EBSAuthApi.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationOptions _options;

        private readonly IJwtGenerator _jwtGenerator;

        public AuthenticationService(IOptions<AuthenticationOptions> options, IJwtGenerator jwtGenerator)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _jwtGenerator = jwtGenerator ?? throw new ArgumentNullException(nameof(jwtGenerator));
        }

        private async Task<(bool, string?, User)> AuthenticateUserAsync(UserLogin userLogin)
        {
            User user = new();
            bool isSuccess = false;
            string? errorMessage = null;

            // TODO: Call SP

            // temporary now

            if (userLogin.Email == "Mantas@gmail.com" && userLogin.Password == "testas")
            {
                UserInfo userInfo = new()
                {
                    Email = "Mantas@gmail.com",
                    Firstname = "Mantas",
                    Lastname = "Lengvinas",
                    Id = "1",
                    Phone = "667987912"
                };

                List<Claim> userClaims = new();

                userClaims.Add(new Claim(ClaimsConstants.Id, userInfo.Id));
                userClaims.Add(new Claim(ClaimsConstants.Email, userInfo.Email));
                userClaims.Add(new Claim(ClaimsConstants.Firstname, userInfo.Firstname));
                userClaims.Add(new Claim(ClaimsConstants.Lastname, userInfo.Lastname));
                userClaims.Add(new Claim(ClaimsConstants.Phone, userInfo.Phone));

                user.UserInfo = userInfo;
                user.UserClaims = userClaims;

                isSuccess = true;
            }
            else
            {
                errorMessage = "Password is incorrect";
            }

            return (isSuccess, errorMessage, user);
        }

        public async Task<AuthResponseDto> LoginUserAsync(UserLogin userLogin, CancellationToken cancelToken)
        {
            AuthResponseDto response = new();

            if (userLogin == null)
            {
                response.ErrorMessage = "No user login information";
                return response;
            }

            if(string.IsNullOrEmpty(userLogin.Email) && string.IsNullOrEmpty(userLogin.Password))
            {
                response.ErrorMessage = "Missing credentials";
                return response;
            }

            (bool authSuccess, string authErrorMessage, User user) = await AuthenticateUserAsync(userLogin);

            if (!authSuccess)
            {
                response.ErrorMessage = authErrorMessage;
                return response;
            }

            string sessionToken = _jwtGenerator.CreateSessionToken(user, _options.Audience, _options.Issuer, _options.ExpirationTimeInSeconds);

            response.SessionToken = sessionToken;
            response.IsSuccess = true;

            return response;
            
        }
    }
}

