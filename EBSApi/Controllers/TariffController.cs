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

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetLatestTariffsAsync(int year, int month, int providerId)
        {
            Response<IEnumerable<Tariff>> response = await _tariffQueries.GetLatestTariffsByMonthAsync(year, month, providerId);

            if (response.IsSuccess == false)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }
    }
}