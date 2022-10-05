using System;
using EBSApi.Models.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EBSApi.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EBSApi.Services.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AuthenticationOptions _options;
        private readonly SymmetricSecurityKey _key;

        public AuthenticationService(IOptions<AuthenticationOptions> options, SymmetricSecurityKey key)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
            _key = key ?? throw new ArgumentNullException(nameof(key));
        }

        public string GetJwt(User user, CancellationToken cancelToken)
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
                Audience = _options.Audience,
                SigningCredentials = new SigningCredentials
                (
                    _key,
                    SecurityAlgorithms.HmacSha512Signature
                )
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string stringToken = tokenHandler.WriteToken(token);

            return stringToken;
        }
    }
}

