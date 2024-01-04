using EBSApp.Models;
using EBSApp.Models.Dtos;

namespace EBSApp.Services
{
    public interface IUsersService
    {
        Task<ApiResponse<List<GetUsersResponseDto>>> GetUsersAsync();
        Task DeleteUsersAsync(int userId);
    }
}