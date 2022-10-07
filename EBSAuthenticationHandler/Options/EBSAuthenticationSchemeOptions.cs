using System;
using Microsoft.AspNetCore.Authentication;

namespace EBSAuthenticationHandler.Options
{
    public class EBSAuthenticationSchemeOptions : RemoteAuthenticationOptions
    {
        public string AuthUrl { get; set; }

        public string AuthAppUrl { get; set; }

        public string AuthApiUrl { get; set; }

        public string ApiKey { get; set; }

        public bool RequestRefreshToken { get; set; }

        public bool IsPersistent { get; set; }

        public string TokenIssuer { get; set; }

        public string TokenAudience { get; set; }

        public string TokenPublicSigningKey { get; set; }

        public int TOkenExpirationInSeconds { get; set; }

        public HttpClient ApiClient { get; set; }
    }
}

