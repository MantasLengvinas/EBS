using EBSApi.Data;
using EBSApi.Models;
using EBSApi.Models.Dtos;

namespace EBSApi.Services
{
    public class TariffServices : ITariffServices
    {
        public IEnumerable<Rate> CalculatePrognosisForYear(IEnumerable<Tariff> tariffs)
        {
            // some smelly code to perform dumb analysis for the year :)

            // months 1-4 tendency to rise by 10% of avg
            // months 5-8 tendency to stay
            // months 9-12 tendency to rise by 5% of avg

            // grab average of all 4 rates
            double averageRateAmbiguous = Math.Round(tariffs.Average(tariff => tariff.Rate), 3);
            double averageRateDay = Math.Round(tariffs.Average(tariff => tariff.RateDay), 3);
            double averageRateEvening = Math.Round(tariffs.Average(tariff => tariff.RateEvening), 3);
            double averageRateNight = Math.Round(tariffs.Average(tariff => tariff.RateNight), 3);

            // no matter how many months, work off of average based on previous tendencies

            List<Rate> rates = new List<Rate>();

            for (int i = 1; i <= 12; i++)
            {
                if(rates.Count() >= 9)
                {
                    rates.Add(new Rate()
                    {
                        RateAmbiguous = Math.Round(averageRateAmbiguous * 1.05, 3),
                        RateDay = Math.Round(averageRateDay * 1.05, 3),
                        RateEvening = Math.Round(averageRateEvening * 1.05, 3),
                        RateNight = Math.Round(averageRateNight * 1.05, 3),
                        OnDate = new DateTime(DateTime.Now.Year, i, 1),
                    });
                }
                else if(rates.Count() >= 5)
                {
                    rates.Add(new Rate()
                    {
                        RateAmbiguous = averageRateAmbiguous,
                        RateDay = averageRateDay,
                        RateEvening = averageRateEvening,
                        RateNight = averageRateNight,
                        OnDate = new DateTime(DateTime.Now.Year, i, 1)
                    });
                }
                else
                {
                    rates.Add(new Rate()
                    {
                        RateAmbiguous = Math.Round(averageRateAmbiguous * 1.10, 3),
                        RateDay = Math.Round(averageRateDay * 1.10, 3),
                        RateEvening = Math.Round(averageRateEvening * 1.10, 3),
                        RateNight = Math.Round(averageRateNight * 1.10, 3),
                        OnDate = new DateTime(DateTime.Now.Year, i, 1)
                    });
                }
            }

            return rates;
        }
    }
}
