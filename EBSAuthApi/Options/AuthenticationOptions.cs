using System;
namespace EBSAuthApi.Options
{
    public class AuthenticationOptions
    {
        public const string Position = "Jwt";
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public int ExpirationTimeInSeconds { get; set; }
    }
}

