using System;
namespace EBSApi.Options
{
    public class AuthenticationOptions
    {
        public const string Position = "Jwt";
        public string? Issuer { get; set; }
        public string? Audience { get; set; }
        public string? Key { get; set; }
    }
}

