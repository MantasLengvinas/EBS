using System;
using EBSApp.Models.Authentication;

namespace EBSApp.Services.General
{
    public class AuthService 
    {
        private readonly HttpClient _httpClient;

        public AuthService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task LoginUserAsync(UserLogin userLogin)
        {
            
        }
    }
}

