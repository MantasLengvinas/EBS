using EBSApp.Models;
using EBSApp.Models.Dtos;

namespace EBSApp.Services
{
    public interface IUsageService
    {
        Task<ApiResponse<GetUsageResponseDto>> GetUnpaidUsages(int addressId);
        Task<ApiResponse<GetUsageResponseDto>> GetUsageHistory(int addressId);
        Task NewUsage(UsageDto usage);
        Task PayUsage(int id);
    }
}