using System;
using System.Collections.Specialized;
using System.Web;
using EBSAuthenticationHandler.Constants;
using EBSAuthenticationHandler.Defaults;
using EBSAuthenticationHandler.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EBSAuthenticationHandler.Services
{
    internal class TokenRefreshService : ITokenRefreshService
    {
        private readonly HttpClient _client;
        private readonly ILogger<TokenRefreshService> _logger;
        private readonly int _tokenExpiration;

        private bool _isLoading = false;

        protected TokenRefreshService(ILogger<TokenRefreshService> logger, int tokenExpiration)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            if (tokenExpiration <= 0)
                throw new ArgumentException(nameof(tokenExpiration) + "cannot be negative");

            _tokenExpiration = tokenExpiration;
        }

        public TokenRefreshService(ILogger<TokenRefreshService> logger, HttpClient client, int tokenExpiration)
            : this(logger, tokenExpiration)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public TokenRefreshService(ILogger<TokenRefreshService> logger, string baseUri, string apiKey, int tokenExpiration)
            : this(logger, tokenExpiration)
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri(baseUri);
            _client.DefaultRequestHeaders.Add(EBSAuthenticationDefaults.ApiKeyHeaderName, apiKey);
        }

        public async Task<string> RefreshAccessToken(string refreshToken)
        {
            if (_isLoading) return null;
            _isLoading = true;

            NameValueCollection tokensQueryString = new();
            tokensQueryString.Add("refreshToken", refreshToken);
            tokensQueryString.Add("tokenTypes", TokenTypesConstants.AccessToken);

            HttpRequestMessage message = new(
                HttpMethod.Get,
                $"{EBSAuthenticationDefaults.RetrieveTokensUrl}?{tokensQueryString}");

            AuthTokensResponse tokensResponse;

            try
            {
                HttpResponseMessage response = await _client.SendAsync(message);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                tokensResponse = JsonConvert.DeserializeObject<AuthTokensResponse>(content);

                if (tokensResponse == null)
                    throw new Exception("Failed to refresh access token");

                _isLoading = false;
                return tokensResponse.AccessToken;
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
            }

            _isLoading = false;
            return null;
        }
    }
}

