using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Services.General;

namespace EBSApp.Services
{
    public class TariffService : ITariffService
    {
        private readonly IApiClient _apiClient;

        public TariffService(IApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public async Task<ApiResponse<List<GetTariffResponseDto>>> GetLatestTariff(int providerId)
        {
            if(providerId == 1)
                return await _apiClient.GetAsync<List<GetTariffResponseDto>>($"api/tariff?year=2022&month=9&providerId={providerId}");
            return await _apiClient.GetAsync<List<GetTariffResponseDto>>($"api/tariff?year=2022&month=10&providerId={providerId}");
        }
    }
}

