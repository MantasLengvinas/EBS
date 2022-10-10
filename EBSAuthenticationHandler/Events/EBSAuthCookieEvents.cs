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
using Microsoft.AspNetCore.Authentication.Cookies;

namespace EBSAuthenticationHandler.Events
{
    public class EBSAuthCookieEvents : CookieAuthenticationEvents
    {
        private const string TicketExpirationTime = nameof(TicketExpirationTime);
        private readonly EBSAuthenticationSchemeOptions _options;

        public EBSAuthCookieEvents(
            IOptions<EBSAuthenticationSchemeOptions> options)
        {
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public override async Task SigningIn(CookieSigningInContext context)
        {
            try
            {
                Claim expClaim = context.Principal.Claims.FirstOrDefault(x => x.Type == "exp");

                context.Properties.SetString(
                    TicketExpirationTime,
                    expClaim.Value);

                await base.SigningIn(context);
            }
            catch(Exception){}
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            string ticketExpirationTime = context
                .Properties.GetString(TicketExpirationTime);

            if (ticketExpirationTime is null ||
                !long.TryParse(ticketExpirationTime, out var ticketExpirationValue))
            {
                await RejectPrincipalAsync(context);
                return;
            }

            var ticketExpirationUtc = DateTimeOffset.FromUnixTimeSeconds(ticketExpirationValue);

            if (DateTime.UtcNow > ticketExpirationUtc)
            {
                await RejectPrincipalAsync(context);
                return;
            }

            await base.ValidatePrincipal(context);
        }

        public override Task RedirectToReturnUrl(RedirectContext<CookieAuthenticationOptions> context)
        {
            return base.RedirectToReturnUrl(context);
        }

        private static async Task RejectPrincipalAsync(CookieValidatePrincipalContext context)
        {
            context.RejectPrincipal();
            AuthenticationProperties properties = new();
            properties.RedirectUri = "./login?redirectUri=/home";
            await context.HttpContext.SignOutAsync(properties);
        }

        //private async Task<(AuthTokensResponse, HandleRequestResult)> GetTokensAsync(string sessionToken)
        //{
        //    NameValueCollection tokensQueryString = new NameValueCollection();

        //    tokensQueryString.Add("sessionToken", sessionToken);
        //    tokensQueryString.Add("tokenTypes", TokenTypesConstants.AccessToken);

        //    if (Options.RequestRefreshToken)
        //        tokensQueryString.Add("tokenTypes", TokenTypesConstants.RefreshToken);

        //    HttpRequestMessage message = new HttpRequestMessage(
        //        HttpMethod.Get, $"{EBSAuthenticationDefaults.RetrieveTokensUrl}?{tokensQueryString}");

        //    AuthTokensResponse tokens;

        //    try
        //    {
        //        HttpResponseMessage response = await _options.ApiClient.SendAsync(message);

        //        response.EnsureSuccessStatusCode();

        //        string content = await response.Content.ReadAsStringAsync();
        //        tokens = JsonConvert.DeserializeObject<AuthTokensResponse>(content);

        //        if (tokens == null)
        //            throw new Exception("Failed to get tokens");

        //    }
        //    catch(Exception e)
        //    {
        //        _logger.LogError(e.Message);
        //        return (null, HandleRequestResult.Fail("Failed to get tokens"));
        //    }

        //    return (tokens, null);
        //}

        //private (AuthenticationTicket, HandleRequestResult) GetAuthenticationTicket(AuthTokensResponse tokens, string returnUrl)
        //{
        //    AuthenticationProperties authProperties = new();

        //    string currentUrl = CurrentUri.Split('?', 2)[0];
        //    string callbackPath = _options.CallbackPath.Value.Remove(_options.CallbackPath.Value.IndexOf('/'), 1);
        //    Uri redirectUrl = new Uri(currentUrl.Replace(callbackPath, ""));

        //    if (!string.IsNullOrEmpty(returnUrl)) redirectUrl = new Uri(redirectUrl, returnUrl);

        //    authProperties.RedirectUri = Uri.EscapeUriString(redirectUrl.ToString());
        //    authProperties.IsPersistent = _options.IsPersistent;

        //    bool accessTokenIsValid = JwtHelper.IsValidToken(
        //        tokens.AccessToken,
        //        _options.TokenPublicSigningKey,
        //        _options.TokenAudience,
        //        _options.TokenIssuer);

        //    if (!accessTokenIsValid)
        //    {
        //        return (null, HandleRequestResult.Fail("Token is invalid"));
        //    }

        //    (ClaimsPrincipal principal, HandleRequestResult result) = GetClaimsPrincipal(tokens.AccessToken);

        //    if (result != null) return (null, result);

        //    JwtPayload accessTokenPayload = JwtHelper.GetJwtSecurityToken(tokens.AccessToken).Payload;
        //    int expiration = accessTokenPayload.Exp ?? throw new Exception("Missing expiration value in payload");
        //    authProperties.ExpiresUtc = DateTimeOffset.FromUnixTimeSeconds(expiration).UtcDateTime;

        //    List<AuthenticationToken> tokensToStore = new List<AuthenticationToken>();
        //    tokensToStore.Add(new AuthenticationToken {
        //        Name = TokenTypesConstants.AccessToken,
        //        Value = tokens.AccessToken
        //    });

        //    if (_options.RequestRefreshToken)
        //    {
        //        bool refreshTokenIsValid = JwtHelper.IsValidToken(
        //            tokens.RefreshToken,
        //            _options.TokenPublicSigningKey,
        //            _options.TokenAudience,
        //            _options.TokenIssuer);

        //        if (!refreshTokenIsValid)
        //            return (null, HandleRequestResult.Fail("refresh token validation failed"));

        //        tokensToStore.Add(new AuthenticationToken
        //        {
        //            Name = TokenTypesConstants.RefreshToken,
        //            Value = tokens.RefreshToken
        //        });
        //    }

        //    if(principal == null)
        //    {
        //        return (null, HandleRequestResult.Fail("Claims principal is null"));
        //    }

        //    AuthenticationTicket ticket = new AuthenticationTicket(principal, authProperties, EBSAuthenticationDefaults.AuthenticationScheme);

        //    return (ticket, null);

        //}

        //private (ClaimsPrincipal, HandleRequestResult) GetClaimsPrincipal(string accessToken)
        //{
        //    IEnumerable<Claim> claims = JwtHelper.GetJwtSecurityToken(accessToken).Claims;

        //    ClaimsIdentity identity = new ClaimsIdentity(
        //        claims,
        //        EBSAuthenticationDefaults.AuthenticationScheme,
        //        ClaimTypesConstants.Email,
        //        null);

        //    if (identity.Name == null)
        //        throw new Exception("Missing claim");

        //    ClaimsPrincipal principal = new(identity);

        //    return (principal, null);
        //}
    }
}

