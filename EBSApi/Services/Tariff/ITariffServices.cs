using EBSApi.Models;
using EBSApi.Models.Dtos;

namespace EBSApi.Services


{
    public interface ITariffServices
    {
        public IEnumerable<Rate> CalculatePrognosisForYear(IEnumerable<Tariff> tariffs);
    }
}
