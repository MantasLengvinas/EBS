using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace EBSApp.Auth
{
    public class EBSAuthenticationStateProvider : AuthenticationStateProvider
    {

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            ClaimsIdentity identity = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, "Mantas"),
            }, "Fake authentication type");

            ClaimsPrincipal user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }
    }
}

