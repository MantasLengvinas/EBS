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
        private readonly RsaSecurityKey _key;

        public AuthenticationService(IOptions<AuthenticationOptions> options, RsaSecurityKey key)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _key = key ?? throw new ArgumentNullException(nameof(key));
        }

        private UserJwt GetJwt(User user, CancellationToken cancelToken)
        {
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim("Email", user.Email),
                    new Claim("FullName", user.FullName),
                    new Claim("Phone", user.Phone)
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                Issuer = _options.Issuer,
                Audience = _options.Audience
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string stringToken = tokenHandler.WriteToken(token);

            UserJwt jwt = new()
            {
                Jwt = stringToken
            };

            return jwt;
        }

        public async Task<UserJwt> AuthenticateUser(UserLogin userLogin, CancellationToken cancelToken)
        {
            if(userLogin.Email == "Mantas@gmail.com" && userLogin.Password == "testas")
            {
                User user = new()
                {
                    Email = "Mantas@gmail.com",
                    FullName = "Mantas Lengvinas",
                    Id = 1,
                    Phone = "667987912"
                };

                return GetJwt(user, cancelToken);
            }

            return null;
        }
    }
}

