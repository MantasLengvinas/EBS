using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;
using EBSApi.Models.Dtos;

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
            Response<IEnumerable<Address>> response = await _addressQueries.GetAllAddressesAsync();

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAddressAsync([FromBody]Address address, CancellationToken cancellationToken)
        {
            Response<Address> response = await _addressQueries.CreateAddressAsync(address);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAddresesUserAsync(int userId, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Address>> response = await _addressQueries.GetAddressesUserAsync(userId);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("provider/{providerId}")]
        public async Task<IActionResult> GetAddresesProviderAsync(int providerId, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Address>> response = await _addressQueries.GetAddressesProviderAsync(providerId);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressAsync(int addressId, CancellationToken cancellationToken)
        {
            Response<Address> response = await _addressQueries.GetAddressAsync(addressId);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpDelete("{addressId}")]
        public async Task<IActionResult> DeleteAddressAsync(int addressId, CancellationToken cancellationToken)
        {
            int response = await _addressQueries.DeleteAddressAsync(addressId);

            if (response != 0)
                return BadRequest();
            return Ok();
        }
    }
}
