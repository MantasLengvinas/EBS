using EBSApi.Models;

namespace EBSApi.Data
{
    public interface IUsageQueries
    {
        public Task<Usage> GetUsageAsync(int id);
        public Task<IEnumerable<Usage>> GetUserUsagesForMonthAsync(int year, int month, int userId);
        public Task<IEnumerable<Usage>> GetAddressUsagesForMonthAsync(int year, int month, int addressId);
        public Task<IEnumerable<Usage>> GetAllUsagesAsync();
        public Task<int> SetUsagePaid(int id);
    }
}
