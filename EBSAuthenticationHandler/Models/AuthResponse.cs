using System;
using Newtonsoft.Json;

namespace EBSAuthenticationHandler.Models
{
    internal class AuthResponse
    {
        [JsonProperty("isSuccess")]
        public bool IsSuccess { get; set; }
        [JsonProperty("sessionToken")]
        public string? SessionToken { get; set; }
        [JsonProperty("errorMessage")]
        public string? ErrorMessage { get; set; }
    }
}

