using EBSApp.Models;
using EBSApp.Models.Dtos;
using EBSApp.Models.Dtos.Responses;

namespace EBSApp.Services
{
    public interface IAddressService
    {
        Task<ApiResponse<List<GetAddressResponseDto>>> GetAllAddressesAsync();
        Task<ApiResponse<List<GetAddressResponseDto>>> GetUserAddresses(string userId);
        Task AddNewAddress(GetAddressResponseDto address);
        Task DeleteAddress(int id);
    }
}