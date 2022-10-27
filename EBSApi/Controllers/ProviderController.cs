using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;

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
            IEnumerable<Provider> providers = await _providerQueries.GetAllProvidersAsync();
            if (providers == null || !providers.Any())
                return NotFound();
            return Ok(providers);
        }

        [HttpGet("{providerId}")]
        public async Task<IActionResult> GetProviderAsync(int providerId, CancellationToken cancellationToken)
        {
            Provider provider = await _providerQueries.GetProviderAsync(providerId);
            if (provider == null)
                return NotFound();
            return Ok(provider);
        }
    }
}
