using EBSApi.Models;

namespace EBSApi.Data
{
    public interface IAddressQueries
    {
        public Task<IEnumerable<Address>> GetAllAddressesAsync();
        public Task<IEnumerable<Address>> GetAddressesProviderAsync(int providerId);
        public Task<IEnumerable<Address>> GetAddressesUserAsync(int userId);
        public Task<Address> GetAddressAsync(int addressId);
    }
}
