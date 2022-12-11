using System;
using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Services.General;

namespace EBSApp.Services
{
    public class UsersService : IUsersService
    {
        private readonly IApiClient _apiClient;

        public UsersService(IApiClient apiClient)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
        }

        public async Task<ApiResponse<List<GetUsersResponseDto>>> GetUsersAsync()
        {
            return await _apiClient.GetAsync<List<GetUsersResponseDto>>($"api/user");
        }

        public async Task DeleteUsersAsync(int userId)
        {
            await _apiClient.DeleteAsync($"api/user/{userId}");
        }
    }
}

