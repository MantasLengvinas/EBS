using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using EBSAuthenticationHandler.Constants;
using EBSAuthenticationHandler.Defaults;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace EBSAuthenticationHandler.Helpers
{
    public class JwtHelper
    {
        public static JwtSecurityToken GetJwtSecurityToken(string token)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return handler.ReadJwtToken(token);
        }

        public static bool IsValidToken(string? token, string key, string audience, string issuer)
        {
            if (token == null)
                return false;

            RSACryptoServiceProvider rsa = new();

            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(key), out _);

            JwtSecurityTokenHandler handler = new();

            TokenValidationParameters validationParams = new()
            {
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new RsaSecurityKey(rsa)
            };

            try
            {
                handler.ValidateToken(token, validationParams, out _);
            }
            catch (SecurityTokenException)
            {
                return false;
            }

            return true;
        }

        public static ClaimsPrincipal? GetClaimsPrincipal(string? accessToken)
        {
            if (accessToken == null)
                return null;

            IEnumerable<Claim> claims = JwtHelper.GetJwtSecurityToken(accessToken).Claims;

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                EBSAuthenticationDefaults.AuthenticationScheme,
                ClaimTypesConstants.Email,
                null);

            if (identity.Name == null)
                throw new Exception("Missing claim");

            ClaimsPrincipal principal = new(identity);

            return principal;
        }
    }
}

