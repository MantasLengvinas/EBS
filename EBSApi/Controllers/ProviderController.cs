using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;
using EBSApi.Models.Dtos;

namespace EBSApi.Controllers
{
    [Route("api/provider")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly IProviderQueries _providerQueries;

        public ProviderController(IProviderQueries providerQueries)
        {
            _providerQueries = providerQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProvidersAsync(CancellationToken cancellationToken)
        {
            Response<IEnumerable<Provider>> response = await _providerQueries.GetAllProvidersAsync();
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpGet("{providerId}")]
        public async Task<IActionResult> GetProviderAsync(int providerId, CancellationToken cancellationToken)
        {
            Response<Provider> response = await _providerQueries.GetProviderAsync(providerId);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }
    }
}
