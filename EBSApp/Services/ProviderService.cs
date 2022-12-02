using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Services.General;

namespace EBSApp.Services
{
    public class ProviderService : IProviderService
    {
        private readonly IApiClient _apiClient;

        public ProviderService(IApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public async Task<ApiResponse<List<GetProviderResponseDto>>> GetProviders()
        {

            return await _apiClient.GetAsync<List<GetProviderResponseDto>>($"api/provider");
        }
    }
}

