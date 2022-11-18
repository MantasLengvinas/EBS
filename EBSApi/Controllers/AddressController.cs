﻿using EBSApi.Models;
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
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAddresesUserAsync(int userId, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Address>> response = await _addressQueries.GetAddressesUserAsync(userId);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("provider/{providerId}")]
        public async Task<IActionResult> GetAddresesProviderAsync(int providerId, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Address>> response = await _addressQueries.GetAddressesProviderAsync(providerId);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("{addressId}")]
        public async Task<IActionResult> GetAddressAsync(int addressId, CancellationToken cancellationToken)
        {
            Response<Address> response = await _addressQueries.GetAddressAsync(addressId);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }
    }
}