using EBSApi.Models;
using EBSApi.Models.Dtos;

namespace EBSApi.Data
{
    public interface IAddressQueries
    {
        public Task<Response<IEnumerable<Address>>> GetAllAddressesAsync();
        public Task<IEnumerable<Address>> GetAddressesProviderAsync(int providerId);
        public Task<IEnumerable<Address>> GetAddressesUserAsync(int userId);
        public Task<Address> GetAddressAsync(int addressId);
    }
}
