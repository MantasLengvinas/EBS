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

        public async Task<string> AuthenticateUser(UserLogin userLogin, CancellationToken cancelToken)
        {
            if(userLogin.Email == "Mantas@gmail.com" && userLogin.Password == "testas")
            {
                User user = new()
                {
                    Email = "Mantas@gmail.com",
                    FullName = "Mantas Lengvinas",
                    Id = "1",
                    Phone = "667987912"
                };

                string accessToken = _jwtGenerator.CreateAccessToken(user, _options.Audience, _options.Issuer, 3600);

                return accessToken;
            }

            return null;
        }
    }
}

