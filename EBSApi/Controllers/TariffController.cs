using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;
using EBSApi.Models.Dtos;
using Microsoft.AspNetCore.Cors;
using EBSApi.Services;
using System.ComponentModel.DataAnnotations;

namespace EBSApi.Controllers
{
    [Route("api/tariff")]
    [ApiController]
    public class TariffController : ControllerBase
    {
        private readonly ITariffQueries _tariffQueries;
        private readonly ITariffServices _tariffServices;

        public TariffController(ITariffQueries tariffQueries, ITariffServices tariffServices)
        {
            _tariffQueries = tariffQueries;
            _tariffServices = tariffServices;
        }

        [HttpGet("{tariffId}")]
        public async Task<IActionResult> GetTariffByIdAsync([Required] int tariffId, CancellationToken cancellationToken)
        {
            Response<Tariff> response = await _tariffQueries.GetTariffByIdAsync(tariffId);

            if (!response.IsSuccess)
                return BadRequest(response);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }
        [EnableCors("Localhost")]
        [HttpGet("/provider/{providerId}/prognosis")]
        public async Task<IActionResult> GetTariffPrognosis([Required]int providerId, bool isBusiness, CancellationToken cancellationToken)
        {
            Response<IEnumerable<Tariff>> queryResult = await _tariffQueries.GetHistoricalTariffDataAsync(providerId, isBusiness);
          
            if (!queryResult.IsSuccess)
                return BadRequest(queryResult);
            if (queryResult.Data == null || !queryResult.Data.Any())
                return NotFound();

            Response<IEnumerable<Rate>> response = new Response<IEnumerable<Rate>>()
            {
                Data = _tariffServices.CalculatePrognosisForYear(queryResult.Data),
                IsSuccess = queryResult.IsSuccess,
                Error = queryResult.Error
            };
            return Ok(response);
        }
        [HttpGet]
        public async Task<IActionResult> GetLatestTariffsAsync([Required] int year, [Required] int month, [Required] int providerId)
        {
            Response<IEnumerable<Tariff>> response = await _tariffQueries.GetLatestTariffsByMonthAsync(year, month, providerId);

            if (!response.IsSuccess)
                return BadRequest(response);
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }
    }
}