using System;
namespace EBSAuthenticationHandler.Models
{
    internal class AuthTokensResponse
    {
        public string IdToken { get; set; }
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}

