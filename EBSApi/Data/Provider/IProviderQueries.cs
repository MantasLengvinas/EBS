using EBSApi.Models;
namespace EBSApi.Data
{
    public interface IProviderQueries
    {
        public Task<Provider> GetProviderAsync(int providerId);
        public Task<IEnumerable<Provider>> GetAllProvidersAsync();
    }
}
