using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace EBSApi.Controllers
{
    [Route("api/address")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressQueries _addressQueries;

        public AddressController(IAddressQueries addressQueries)
        {
            _addressQueries = addressQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAddressesAsync(CancellationToken cancellationToken)
        {
            IEnumerable<Address> addresses = (await _addressQueries.GetAllAddressesAsync()).Data;
            if (addresses == null || !addresses.Any())
                return NotFound();
            return Ok(addresses);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAddresesUserAsync(int userId, CancellationToken cancellationToken)
        {
            IEnumerable<Address> addresses = await _addressQueries.GetAddressesUserAsync(userId);
            if (addresses == null || !addresses.Any())
                return NotFound();
            return Ok(addresses);
        }

        [HttpGet("provider/{providerId}")]
        public async Task<IActionResult> GetAddresesProviderAsync(int providerId, CancellationToken cancellationToken)
        {
            IEnumerable<Address> addresses = await _addressQueries.GetAddressesProviderAsync(providerId);
            if (addresses == null || !addresses.Any())
                return NotFound();
            return Ok(addresses);
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressAsync(int addressId, CancellationToken cancellationToken)
        {
            Address address = await _addressQueries.GetAddressAsync(addressId);
            if (address == null)
                return NotFound();
            return Ok(address);
        }
    }
}
