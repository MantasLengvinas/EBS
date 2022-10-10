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
    }
}

