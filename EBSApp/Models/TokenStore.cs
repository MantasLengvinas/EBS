using System;
namespace EBSApp.Models
{
    public class TokenStore
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}