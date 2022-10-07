using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EBSAuthApi.Constants;
using EBSAuthApi.Models;
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

        private string CreateJwt(List<Claim> claims, string audience, string issuer, int expiration)
        {
            SecurityTokenDescriptor token = new SecurityTokenDescriptor()
            {
                TokenType = "JWT",
                Subject = new ClaimsIdentity(claims),
                Audience = audience,
                Issuer = issuer,
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddSeconds(expiration),
                SigningCredentials = new SigningCredentials(
                    _key,
                    SecurityAlgorithms.RsaSha256
                    )
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken = tokenHandler.CreateToken(token);

            return tokenHandler.WriteToken(securityToken);
        }

        public string CreateAccessToken(User user, string audience, string issuer, int expiration)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimsConstants.Id, user.Id));
            claims.Add(new Claim(ClaimsConstants.Email, user.Email));
            claims.Add(new Claim(ClaimsConstants.FullName, user.FullName));
            claims.Add(new Claim(ClaimsConstants.Phone, user.Phone));

            return CreateJwt(claims, audience, issuer, expiration);
        }

        public string CreateIdToken(string accessToken, string email, string audience, string issuer, int expiration)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim(ClaimsConstants.Email, email));
            claims.Add(new Claim("accessToken", accessToken));
            claims.Add(new Claim("refreshToken", CreateRefreshToken(accessToken, audience, issuer, expiration)));

            return CreateJwt(claims, audience, issuer, expiration);
        }

        public string CreateRefreshToken(string accessToken, string audience, string issuer, int expiration)
        {
            List<Claim> claims = new List<Claim>();

            claims.Add(new Claim("accessToken", accessToken));

            return CreateJwt(claims, audience, issuer, expiration);
        }
    }
}

