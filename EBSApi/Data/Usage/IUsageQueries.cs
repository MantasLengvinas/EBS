using EBSApi.Models;
using EBSApi.Models.Dtos;

namespace EBSApi.Data
{
    public interface IUsageQueries
    {
        public Task<Response<Usage>> GetUsageAsync(int id);
        public Task<Response<IEnumerable<Usage>>> GetUserUsagesForMonthAsync(int year, int month, int userId);
        public Task<Response<IEnumerable<Usage>>> GetAddressUsagesForMonthAsync(int year, int month, int addressId);
        public Task<Response<IEnumerable<Usage>>> GetAllUsagesAsync();
        public Task<Response<int>> SetUsagePaid(int id);
    }
}
