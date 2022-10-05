using System;
using EBSApp.Models.Authentication;
using EBSApp.Options;
using EBSApp.Services.General;
using Microsoft.Extensions.Options;

namespace EBSApp.Services.Authentication
{
    public class AuthenticationService
    {
        private readonly IApiClient _apiClient;
        private readonly ApiClientOptions _options;
        private readonly bool _isDevelopment;

        public AuthenticationService(IApiClient apiClient, IOptions<ApiClientOptions> options, bool isDevelopment)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task AuthenticateUserAsync(UserLogin user)
        {
            
        }
    }
}

