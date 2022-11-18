using EBSApi.Models.Dtos;
using EBSApi.Models;

namespace EBSApi.Data
{
    public interface ITariffQueries
    {
        public Task<Response<Tariff>> GetTariffByIdAsync(int id);
        public Task<Response<IEnumerable<Tariff>>> GetLatestTariffsByMonthAsync(int year, int month, int providerId);
    }
}
