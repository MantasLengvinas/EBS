using EBSApi.Models;
using EBSApi.Models.Dtos;

namespace EBSApi.Data
{
    public interface IAddressQueries
    {
        public Task<Response<IEnumerable<Address>>> GetAllAddressesAsync();
        public Task<Response<IEnumerable<Address>>> GetAddressesProviderAsync(int providerId);
        public Task<Response<IEnumerable<Address>>> GetAddressesUserAsync(int userId);
        public Task<Response<Address>> GetAddressAsync(int addressId);
    }
}
