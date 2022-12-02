using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Models.Dtos.Responses;
using EBSApp.Services.General;

namespace EBSApp.Services
{
    public class AddressService : IAddressService
    {
        private readonly IApiClient _apiClient;

        public AddressService(IApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public async Task<ApiResponse<List<GetAddressResponseDto>>> GetUserAddresses(string userId)
        {
            
            return await _apiClient.GetAsync<List<GetAddressResponseDto>>($"api/address/user/{userId}");
        }
    }
}

