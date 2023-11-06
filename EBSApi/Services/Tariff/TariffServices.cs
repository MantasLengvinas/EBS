/*
﻿using EBSApi.Data;
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
*/

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
