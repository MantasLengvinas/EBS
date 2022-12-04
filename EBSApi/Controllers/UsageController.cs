using EBSApi.Data;
using EBSApi.Models;
using EBSApi.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EBSApi.Controllers
{
    [Route("api/usage")]
    [ApiController]
    public class UsageController : ControllerBase
    {
        private readonly IUsageQueries _usageQueries;

        public UsageController(IUsageQueries usageQueries)
        {
            _usageQueries = usageQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsagesAsync(CancellationToken cancellationToken)
        {
            Response<IEnumerable<Usage>> response = await _usageQueries.GetAllUsagesAsync();

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUsageAsync([FromBody]Usage usage, CancellationToken cancellationToken)
        {
            Response<Usage> response = await _usageQueries.CreateUsageAsync(usage);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsageAsync(int id, CancellationToken cancellationToken)
        {
            Response<Usage> response = await _usageQueries.GetUsageAsync(id);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserUsagesForMonthAsync(int year, int month, int userId, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Usage>> response = await _usageQueries.GetUserUsagesForMonthAsync(year, month, userId);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("address")]
        public async Task<IActionResult> GetAddressUsagesForMonthAsync(int year, int month, int addressId, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Usage>> response = await _usageQueries.GetAddressUsagesForMonthAsync(year, month, addressId);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("unpaid")]
        public async Task<IActionResult> GetAddressUnpaidUsagesAsync(int id, CancellationToken cancellationToken)
        {
            Response<PaymentDto> response = await _usageQueries.GetAddressUnpaidUsagesAsync(id);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Usages.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpPut("paid/{id}")]
        public async Task<IActionResult> SetUsagePaidAsync(int id, CancellationToken cancellationToken)
        {
            Response<Usage> response = await _usageQueries.SetUsagePaidAsync(id);

            if(response.IsSuccess == true)
                return (Ok(response));
            else return BadRequest(response);
        }
    }
}
