using EBSApi.Models;
using EBSApi.Models.Dtos;

namespace EBSApi.Data
{
    public interface IProviderQueries
    {
        public Task<Response<Provider>> GetProviderAsync(int providerId);
        public Task<Response<IEnumerable<Provider>>> GetAllProvidersAsync();
    }
}
