using EBSApi.Data;
using EBSApi.Models;
using EBSApi.Models.Dtos;
using Microsoft.AspNetCore.Mvc;

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
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsageAsync(int id, CancellationToken cancellationToken)
        {
            Response<Usage> response = await _usageQueries.GetUsageAsync(id);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet("user/{year}/{month}/{userId}")]
        public async Task<IActionResult> GetUserUsagesForMonthAsync(int year, int month, int userId, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Usage>> response = await _usageQueries.GetUserUsagesForMonthAsync(year, month, userId);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("address/{year}/{month}/{addressId}")]
        public async Task<IActionResult> GetAddressUsagesForMonthAsync(int year, int month, int addressId, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Usage>> response = await _usageQueries.GetAddressUsagesForMonthAsync(year, month, addressId);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpPost("paid/id")]
        public async Task<IActionResult> SetUsagePaid(int id)
        {
            Response<int> response = await _usageQueries.SetUsagePaid(id);
            if(response.IsSuccess == true)
                return (Ok(response));
            else return BadRequest();
        }
    }
}
