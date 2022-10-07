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
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EBSAuthenticationHandler.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly HttpClient _client;
        private readonly ILogger<UserAuthService> _logger;

        public UserAuthService(ILogger<UserAuthService> logger, HttpClient client)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public UserAuthService(ILogger<UserAuthService> logger, string baseUri, string apiKey)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUri);
            _client.DefaultRequestHeaders.Add(EBSAuthenticationDefaults.ApiKeyHeaderName, apiKey);
        }

        public async Task<AuthenticateResult> LoginUser(object userCredentials)
        {

            if (userCredentials == null)
                throw new Exception("User credentials cannot be null");

            

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, EBSAuthenticationDefaults.LoginUrl);
            message.Content = new StringContent(JsonConvert.SerializeObject(userCredentials), Encoding.UTF8, "application/json");

            ClaimsPrincipal principal = null;
            AuthenticationProperties authProperties = new();

            authProperties.IsPersistent = true;
            authProperties.ExpiresUtc = DateTime.UtcNow.AddMinutes(30);

            try
            {
                HttpResponseMessage response = await _client.SendAsync(message);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                AuthResponse deserializedRespose = JsonConvert.DeserializeObject<AuthResponse>(content);

                if (deserializedRespose == null)
                    throw new Exception("Failed to deserialize token");

                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(deserializedRespose.SessionToken);

                principal = JwtHelper.GetClaimsPrincipal(deserializedRespose.SessionToken);

            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
            }

            AuthenticationTicket ticket = new AuthenticationTicket(principal, authProperties, EBSAuthenticationDefaults.AuthenticationScheme);

            return AuthenticateResult.Success(ticket);
        }
    }
}

