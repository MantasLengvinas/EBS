using System;
using Newtonsoft.Json;

namespace EBSAuthenticationHandler.Models
{
    internal class AuthResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("sessionToken")]
        public string SessionToken { get; set; }
    }
}

