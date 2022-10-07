using System;
using System.Text.Encodings.Web;
using EBSAuthenticationHandler.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using EBSAuthenticationHandler.Defaults;
using EBSAuthenticationHandler.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;
using System.Web;
using System.Collections.Specialized;
using EBSAuthenticationHandler.Constants;
using System.Security;
using System.Security.Claims;
using EBSAuthenticationHandler.Helpers;
using System.Text;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Features.Authentication;

namespace EBSAuthenticationHandler.Handlers
{
    public class EBSAuthHandler : RemoteAuthenticationHandler<EBSAuthenticationSchemeOptions>
    {
        private readonly ILogger<EBSAuthHandler> _logger;

        public EBSAuthHandler(
            IOptionsMonitor<EBSAuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock)
            :base(options, logger, encoder, clock)
        {
            _logger = logger.CreateLogger<EBSAuthHandler>();
        }

        protected override Task InitializeHandlerAsync()
        {
            if (!string.IsNullOrEmpty(Options.AuthUrl))
            {
                Options.AuthApiUrl = Options.AuthUrl;
                Options.AuthAppUrl = Options.AuthUrl;
            }

            if(Options.ApiClient == null)
            {
                Options.ApiClient = new HttpClient();
                Options.ApiClient.BaseAddress = new Uri(Options.AuthApiUrl);
                Options.ApiClient.DefaultRequestHeaders.Add(
                    EBSAuthenticationDefaults.ApiKeyHeaderName, Options.ApiKey);
            }

            return Task.CompletedTask;
        }

        protected override async Task HandleChallengeAsync(AuthenticationProperties properties)
        {
            if (!string.IsNullOrEmpty(properties.RedirectUri) && !Uri.IsWellFormedUriString(properties.RedirectUri, UriKind.Relative))
                throw new Exception("Redirect url is not correct");

            HttpRequestMessage message = new HttpRequestMessage(HttpMethod.Post, EBSAuthenticationDefaults.LoginUrl);

            try
            {
                HttpResponseMessage response = await Options.ApiClient.SendAsync(message);

                response.EnsureSuccessStatusCode();

                _logger.LogInformation("Token received");

                string content = await response.Content.ReadAsStringAsync();

                AuthResponse result = JsonConvert.DeserializeObject<AuthResponse>(content);

                if (result == null)
                    throw new Exception("Failed to deserialize token");

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return;
            }

            Response.StatusCode = StatusCodes.Status302Found;
            Response.Headers.Add(HeaderNames.Location, properties.RedirectUri);
        }

        protected override async Task<HandleRequestResult> HandleRemoteAuthenticateAsync()
        {
            string queryString = new Uri(CurrentUri).Query;
            NameValueCollection queryDictionary = HttpUtility.ParseQueryString(queryString);
            string sessionToken = queryDictionary["token"];
            string returnUrl = queryDictionary["returnUrl"];

            if (!string.IsNullOrEmpty(returnUrl))
            {
                returnUrl = System.Net.WebUtility.UrlEncode(returnUrl);
                returnUrl = Uri.UnescapeDataString(returnUrl);
            }

            if (string.IsNullOrEmpty(sessionToken))
            {
                return HandleRequestResult.Fail("Session token does not exist");
            }

            AuthTokensResponse tokens;
            AuthenticationTicket ticket;
            HandleRequestResult result;

            (tokens, result) = await GetTokensAsync(sessionToken);
            if (result != null)
                return result;

            (ticket, result) = GetAuthenticationTicket(tokens, returnUrl);
            if (result != null)
                return result;

            return HandleRequestResult.Success(ticket);
        }

        private async Task<(AuthTokensResponse, HandleRequestResult)> GetTokensAsync(string sessionToken)
        {
            NameValueCollection tokensQueryString = new NameValueCollection();

            tokensQueryString.Add("sessionToken", sessionToken);
            tokensQueryString.Add("tokenTypes", TokenTypesConstants.AccessToken);

            if (Options.RequestRefreshToken)
                tokensQueryString.Add("tokenTypes", TokenTypesConstants.RefreshToken);

            HttpRequestMessage message = new HttpRequestMessage(
                HttpMethod.Get, $"{EBSAuthenticationDefaults.RetrieveTokensUrl}?{tokensQueryString}");

            AuthTokensResponse tokens;

            try
            {
                HttpResponseMessage response = await Options.ApiClient.SendAsync(message);

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();
                tokens = JsonConvert.DeserializeObject<AuthTokensResponse>(content);

                if (tokens == null)
                    throw new Exception("Failed to get tokens");

            }
            catch(Exception e)
            {
                _logger.LogError(e.Message);
                return (null, HandleRequestResult.Fail("Failed to get tokens"));
            }

            return (tokens, null);
        }

        private (AuthenticationTicket, HandleRequestResult) GetAuthenticationTicket(AuthTokensResponse tokens, string returnUrl)
        {
            AuthenticationProperties authProperties = new();

            string currentUrl = CurrentUri.Split('?', 2)[0];
            string callbackPath = Options.CallbackPath.Value.Remove(Options.CallbackPath.Value.IndexOf('/'), 1);
            Uri redirectUrl = new Uri(currentUrl.Replace(callbackPath, ""));

            if (!string.IsNullOrEmpty(returnUrl)) redirectUrl = new Uri(redirectUrl, returnUrl);

            authProperties.RedirectUri = Uri.EscapeUriString(redirectUrl.ToString());
            authProperties.IsPersistent = Options.IsPersistent;

            bool accessTokenIsValid = JwtHelper.IsValidToken(
                tokens.AccessToken,
                Options.TokenPublicSigningKey,
                Options.TokenAudience,
                Options.TokenIssuer);

            if (!accessTokenIsValid)
            {
                return (null, HandleRequestResult.Fail("Token is invalid"));
            }

            (ClaimsPrincipal principal, HandleRequestResult result) = GetClaimsPrincipal(tokens.AccessToken);

            if (result != null) return (null, result);

            JwtPayload accessTokenPayload = JwtHelper.GetJwtSecurityToken(tokens.AccessToken).Payload;
            int expiration = accessTokenPayload.Exp ?? throw new Exception("Missing expiration value in payload");
            authProperties.ExpiresUtc = DateTimeOffset.FromUnixTimeSeconds(expiration).UtcDateTime;

            List<AuthenticationToken> tokensToStore = new List<AuthenticationToken>();
            tokensToStore.Add(new AuthenticationToken {
                Name = TokenTypesConstants.AccessToken,
                Value = tokens.AccessToken
            });

            if (Options.RequestRefreshToken)
            {
                bool refreshTokenIsValid = JwtHelper.IsValidToken(
                    tokens.RefreshToken,
                    Options.TokenPublicSigningKey,
                    Options.TokenAudience,
                    Options.TokenIssuer);

                if (!refreshTokenIsValid)
                    return (null, HandleRequestResult.Fail("refresh token validation failed"));

                tokensToStore.Add(new AuthenticationToken
                {
                    Name = TokenTypesConstants.RefreshToken,
                    Value = tokens.RefreshToken
                });
            }

            if(principal == null)
            {
                return (null, HandleRequestResult.Fail("Claims principal is null"));
            }

            AuthenticationTicket ticket = new AuthenticationTicket(principal, authProperties, EBSAuthenticationDefaults.AuthenticationScheme);

            return (ticket, null);
            
        }

        private (ClaimsPrincipal, HandleRequestResult) GetClaimsPrincipal(string accessToken)
        {
            IEnumerable<Claim> claims = JwtHelper.GetJwtSecurityToken(accessToken).Claims;

            ClaimsIdentity identity = new ClaimsIdentity(
                claims,
                EBSAuthenticationDefaults.AuthenticationScheme,
                ClaimTypesConstants.FullName,
                null);

            if (identity.Name == null)
                throw new Exception("Missing claim");

            ClaimsPrincipal principal = new(identity);

            return (principal, null);
        }
    }
}

