using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using EBSAuthenticationHandler.Defaults;
using EBSAuthenticationHandler.Helpers;
using EBSAuthenticationHandler.Models;
using EBSAuthenticationHandler.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EBSAuthenticationHandler.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly HttpClient _client;
        private readonly ILogger<UserAuthService> _logger;
        private readonly EBSAuthenticationSchemeOptions _options;

        public UserAuthService(ILogger<UserAuthService> logger, HttpClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public UserAuthService(ILogger<UserAuthService> logger, EBSAuthenticationSchemeOptions options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _client = new HttpClient();
            _client.BaseAddress = new Uri(_options.AuthApiUrl);
            _client.DefaultRequestHeaders.Add(EBSAuthenticationDefaults.ApiKeyHeaderName, _options.ApiKey);
        }

        public async Task<AuthenticateResult> LoginUser(object userCredentials)
        {

            string? errorMessage = null;

            if (userCredentials == null)
            {
                errorMessage = "No user credentials provided";
                return AuthenticateResult.Fail(errorMessage);
            }

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, EBSAuthenticationDefaults.LoginUrl);
            message.Content = new StringContent(JsonConvert.SerializeObject(userCredentials), Encoding.UTF8, "application/json");

            ClaimsPrincipal? principal = null;
            AuthenticationProperties authProperties = new();

            authProperties.IsPersistent = true;
            authProperties.ExpiresUtc = DateTime.UtcNow.AddSeconds(_options.TokenExpirationInSeconds);

            try
            {
                HttpResponseMessage response = await _client.SendAsync(message);

                string content = await response.Content.ReadAsStringAsync();

                AuthResponse? deserializedRespose = JsonConvert.DeserializeObject<AuthResponse>(content);

                if (deserializedRespose == null)
                    return AuthenticateResult.Fail("Unhandled authentication exception");

                if (!deserializedRespose.IsSuccess)
                {
                    return AuthenticateResult.Fail(deserializedRespose.ErrorMessage);
                }

                principal = ValidateAuthResponse(deserializedRespose);

                if (principal == null)
                    return AuthenticateResult.Fail("Token is invalid");

            }
            catch(Exception)
            {
            }

            if (principal == null)
                return AuthenticateResult.Fail("Unhandled authentication exception");

            AuthenticationTicket ticket = new AuthenticationTicket(principal, authProperties, EBSAuthenticationDefaults.AuthenticationScheme);

            return AuthenticateResult.Success(ticket);
        }

        public async Task<AuthenticateResult> RegisterClient(object userCredentials)
        {
            string? errorMessage = null;

            if (userCredentials == null)
            {
                errorMessage = "No user credentials provided";
                return AuthenticateResult.Fail(errorMessage);
            }

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, EBSAuthenticationDefaults.RegisterUrl);
            message.Content = new StringContent(JsonConvert.SerializeObject(userCredentials), Encoding.UTF8, "application/json");

            ClaimsPrincipal? principal = null;
            AuthenticationProperties authProperties = new();

            authProperties.IsPersistent = true;
            authProperties.ExpiresUtc = DateTime.UtcNow.AddSeconds(_options.TokenExpirationInSeconds);

            try
            {
                HttpResponseMessage response = await _client.SendAsync(message);

                string content = await response.Content.ReadAsStringAsync();

                AuthResponse? deserializedRespose = JsonConvert.DeserializeObject<AuthResponse>(content);

                if (deserializedRespose == null)
                    return AuthenticateResult.Fail("Unhandled authentication exception");

                if (!deserializedRespose.IsSuccess)
                {
                    return AuthenticateResult.Fail(deserializedRespose.ErrorMessage);
                }

                principal = ValidateAuthResponse(deserializedRespose);

                if (principal == null)
                    return AuthenticateResult.Fail("Token is invalid");

            }
            catch (Exception)
            {
            }

            if (principal == null)
                return AuthenticateResult.Fail("Unhandled authentication exception");

            AuthenticationTicket ticket = new AuthenticationTicket(principal, authProperties, EBSAuthenticationDefaults.AuthenticationScheme);

            return AuthenticateResult.Success(ticket);
        }

        public async Task<bool> CompleteUserRegistration(object userInfo)
        {

            if (userInfo == null)
            {
                return false;
            }

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, EBSAuthenticationDefaults.CompleteRegistrationUrl);
            message.Content = new StringContent(JsonConvert.SerializeObject(userInfo), Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await _client.SendAsync(message);

                if (!response.IsSuccessStatusCode)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private ClaimsPrincipal? ValidateAuthResponse(AuthResponse? authResponse)
        {
            ClaimsPrincipal? principal = null;

            if (authResponse == null)
                throw new Exception("Failed to deserialize token");

            if (!JwtHelper.IsValidToken(
                authResponse.SessionToken,
                _options.TokenPublicSigningKey,
                _options.TokenAudience,
                _options.TokenIssuer))
            {
                return null;
            }

            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(authResponse.SessionToken);

            principal = JwtHelper.GetClaimsPrincipal(authResponse.SessionToken);

            return principal;
        }
    }
}

