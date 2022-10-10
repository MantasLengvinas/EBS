using System;
using Microsoft.AspNetCore.Authentication;

namespace EBSAuthenticationHandler.Options
{
    public class EBSAuthenticationSchemeOptions : AuthenticationSchemeOptions
    {
        public string AuthApiUrl { get; set; }

        public string ApiKey { get; set; }

        public bool IsPersistent { get; set; }

        public string TokenIssuer { get; set; }

        public string TokenAudience { get; set; }

        public string TokenPublicSigningKey { get; set; }

        public int TokenExpirationInSeconds { get; set; }

        public HttpClient ApiClient { get; set; }
    }
}

