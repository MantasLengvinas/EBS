using EBSApi.Data;
using EBSApi.Models;
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
            IEnumerable<Usage> usages = await _usageQueries.GetAllUsagesAsync();
            if (usages == null || !usages.Any())
                return NotFound();
            return Ok(usages);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsageAsync(int id, CancellationToken cancellationToken)
        {
            Usage usage = await _usageQueries.GetUsageAsync(id);
            if (usage == null)
                return NotFound();
            return Ok(usage);
        }

        [HttpGet("user/{year}/{month}/{userId}")]
        public async Task<IActionResult> GetUserUsagesForMonthAsync(int year, int month, int userId, CancellationToken cancellationToken)
        {
            IEnumerable<Usage> usages = await _usageQueries.GetUserUsagesForMonthAsync(year, month, userId);
            if (usages == null || !usages.Any())
                return NotFound();
            return Ok(usages);
        }

        [HttpGet("address/{year}/{month}/{addressId}")]
        public async Task<IActionResult> GetAddressUsagesForMonthAsync(int year, int month, int addressId, CancellationToken cancellationToken)
        {
            IEnumerable<Usage> usages = await _usageQueries.GetAddressUsagesForMonthAsync(year, month, addressId);
            if (usages == null || !usages.Any())
                return NotFound();
            return Ok(usages);
        }

        [HttpPost("paid/id")]
        public async Task<IActionResult> SetUsagePaid(int id)
        {
            int result = await _usageQueries.SetUsagePaid(id);
            if(result == 0)
                return (Ok(result));
            else return BadRequest();
        }
    }
}
