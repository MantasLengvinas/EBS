using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;
using EBSApi.Models.Dtos;

namespace EBSApi.Controllers
{
    [Route("api/tariff")]
    [ApiController]
    public class TariffController : ControllerBase
    {
        private readonly ITariffQueries _tariffQueries;

        public TariffController(ITariffQueries tariffQueries)
        {
            _tariffQueries = tariffQueries;
        }

        [HttpGet("{tariffId}")]
        public async Task<IActionResult> GetTariffByIdAsync(int tariffId, CancellationToken cancellationToken)
        {
            Response<Tariff> response = await _tariffQueries.GetTariffByIdAsync(tariffId);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet("{providerId}/{year}/{month}/")]
        public async Task<IActionResult> GetLatesTariffsAsync(int year, int month, int providerId)
        {
            Response<IEnumerable<Tariff>> response = await _tariffQueries.GetLatestTariffsByMonthAsync(year, month, providerId);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }
    }
}