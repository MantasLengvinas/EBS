using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Services.General;

namespace EBSApp.Services
{
    public class UsageService : IUsageService
    {
        private readonly IApiClient _apiClient;

        public UsageService(IApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public async Task<ApiResponse<GetUsageResponseDto>> GetUnpaidUsages(int addressId)
        {
            return await _apiClient.GetAsync<GetUsageResponseDto>($"api/usage/unpaid?id={addressId}");
        }

        public async Task<ApiResponse<GetUsageResponseDto>> GetUsageHistory(int addressId)
        {
            return await _apiClient.GetAsync<GetUsageResponseDto>($"api/usage/history?id={addressId}");
        }

        public async Task NewUsage(UsageDto usage)
        {
            await _apiClient.PostAsync<UsageDto, UsageDto>("api/usage", usage);
        }

        public async Task PayUsage(int id)
        {
            await _apiClient.PutAsync($"api/usage/paid/{id}");
        }
    }
}

