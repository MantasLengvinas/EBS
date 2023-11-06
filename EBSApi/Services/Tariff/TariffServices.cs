using EBSApi.Models;
using EBSApi.Models.Dtos;

namespace EBSApi.Services
{
    public class TariffServices : ITariffServices
    {
        public IEnumerable<Rate> CalculatePrognosisForYear(IEnumerable<Tariff> tariffs)
        {
            var averageRates = CalculateAverageRates(tariffs);
            return GenerateMonthlyRates(averageRates);
        }

        private static AverageRateDto CalculateAverageRates(IEnumerable<Tariff> tariffs)
        {
            List<Tariff> tariffsList = tariffs.ToList();
            return new AverageRateDto
            {
                RateAmbiguous = CalculateAverage(tariffsList, t => t.Rate),
                RateDay = CalculateAverage(tariffsList, t => t.RateDay),
                RateEvening = CalculateAverage(tariffsList, t => t.RateEvening),
                RateNight = CalculateAverage(tariffsList, t => t.RateNight)
            };
        }

        private static double CalculateAverage(IEnumerable<Tariff> tariffs, Func<Tariff, double> selector)
        {
            return Math.Round(tariffs.Average(selector), 3);
        }

        private IEnumerable<Rate> GenerateMonthlyRates(AverageRateDto averages)
        {
            List<Rate> rates = new List<Rate>();
            for (int i = 1; i <= 12; i++)
            {
                var adjustmentFactor = DetermineAdjustmentFactor(i);
                rates.Add(CreateAdjustedRate(averages, i, adjustmentFactor));
            }

            return rates;
        }

        private static double DetermineAdjustmentFactor(int month)
        {
            if (month >= 9) return 1.05;
            return month >= 5 ? 1.00 : 1.10;
        }

        private static Rate CreateAdjustedRate(AverageRateDto averages, int month, double adjustmentFactor)
        {
            return new Rate
            {
                RateAmbiguous = Math.Round(averages.RateAmbiguous * adjustmentFactor, 3),
                RateDay = Math.Round(averages.RateDay * adjustmentFactor, 3),
                RateEvening = Math.Round(averages.RateEvening * adjustmentFactor, 3),
                RateNight = Math.Round(averages.RateNight * adjustmentFactor, 3),
                OnDate = DateTime.SpecifyKind(new DateTime(DateTime.Now.Year, month, 1), DateTimeKind.Local)
            };
        }
    }

    public class AverageRateDto
    {
        public double RateAmbiguous { get; set; }
        public double RateDay { get; set; }
        public double RateEvening { get; set; }
        public double RateNight { get; set; }
    }
}
