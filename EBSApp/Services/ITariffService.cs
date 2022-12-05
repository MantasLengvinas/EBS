using EBSApp.Models;
using EBSApp.Models.Dtos;

namespace EBSApp.Services
{
    public interface ITariffService
    {
        Task<ApiResponse<List<GetTariffResponseDto>>> GetLatestTariff(int providerId);
    }
}