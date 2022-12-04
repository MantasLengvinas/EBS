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
        public Task<Response<Usage>> SetUsagePaidAsync(int id);
        public Task<Response<Usage>> CreateUsageAsync(Usage usage);
        public Task<Response<PaymentDto>> GetAddressUnpaidUsagesAsync(int id);
    }
}
