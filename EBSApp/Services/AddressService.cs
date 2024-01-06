using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Models.Dtos.Responses;
using EBSApp.Pages.Admin;
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

        public async Task<ApiResponse<List<GetAddressResponseDto>>> GetAllAddressesAsync()
        {
            return await _apiClient.GetAsync<List<GetAddressResponseDto>>($"api/address");
        }

        public async Task<ApiResponse<List<GetAddressResponseDto>>> GetUserAddresses(string userId)
        {
            
            return await _apiClient.GetAsync<List<GetAddressResponseDto>>($"api/address/user/{userId}");
        }

        public async Task AddNewAddress(GetAddressResponseDto address)
        {
            await _apiClient.PostAsync<GetAddressResponseDto, GetAddressResponseDto>("api/address", address);
        }

        public async Task<bool> DeleteAddress(int id)
        {
            return await _apiClient.DeleteAsync($"api/address/{id}");
        }
    }
}

