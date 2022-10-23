using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EBSAuthApi.Constants;
using EBSAuthApi.Models;
using EBSAuthApi.Models.Domain;
using Microsoft.IdentityModel.Tokens;

namespace EBSAuthApi.Services
{
    public class JwtGenerator : IJwtGenerator
    {

        private RsaSecurityKey _key;

        public JwtGenerator(RsaSecurityKey key)
        {
            _key = key ?? throw new ArgumentNullException(nameof(key));
        }

        private string CreateJwt(List<Claim> claims, string audience, string issuer, int expirationTimeInSeconds)
        {
            SecurityTokenDescriptor token = new SecurityTokenDescriptor()
            {
                TokenType = "JWT",
                Subject = new ClaimsIdentity(claims),
                Audience = audience,
                Issuer = issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddSeconds(expirationTimeInSeconds),
                SigningCredentials = new SigningCredentials(
                    _key,
                    SecurityAlgorithms.RsaSha256
                    )
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(token);

            return tokenHandler.WriteToken(securityToken);
        }

        private string CreateRefreshToken(string accessToken, string audience, string issuer, int expiration)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("accessToken", accessToken));

            return CreateJwt(claims, audience, issuer, expiration);
        }

        public string CreateSessionToken(User user, string audience, string issuer, int expiration)
        {
            List<Claim> claims = new();

            claims.AddRange(user.UserClaims);
            string accessToken = CreateJwt(user.UserClaims, audience, issuer, expiration);

            claims.Add(new Claim("accessToken", accessToken));

            return CreateJwt(claims, audience, issuer, expiration);
        }
    }
}

