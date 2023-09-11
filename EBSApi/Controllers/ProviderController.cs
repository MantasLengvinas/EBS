using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;
using EBSApi.Models.Dtos;
using Microsoft.AspNetCore.Cors;

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
        [EnableCors("Localhost")]
        public async Task<IActionResult> GetAllProvidersAsync(CancellationToken cancellationToken)
        {
            Response<IEnumerable<Provider>> response = await _providerQueries.GetAllProvidersAsync();

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProviderAsync(Provider provider, CancellationToken cancellationToken)
        {
            Response<Provider> response = await _providerQueries.CreateProviderAsync(provider);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet("{providerId}")]
        public async Task<IActionResult> GetProviderAsync(int providerId, CancellationToken cancellationToken)
        {
            Response<Provider> response = await _providerQueries.GetProviderAsync(providerId);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }
    }
}
