using EBSApp.Models;
using EBSApp.Models.Dtos;

namespace EBSApp.Services
{
    public interface IProviderService
    {
        Task<ApiResponse<List<GetProviderResponseDto>>> GetProviders();
    }
}