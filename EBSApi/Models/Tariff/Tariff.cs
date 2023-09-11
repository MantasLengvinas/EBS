﻿namespace EBSApi.Models
{
    public class Tariff
    {
        public int TariffId { get; set; }
        public DateTime? RegistryDate { get; set; }
        public double Rate { get; set; }
        public double RateDay { get; set; }
        public double RateEvening { get; set; }
        public double RateNight { get; set; }
        public int ProviderId { get; set; }
        public bool IsBusiness { get; set; }
    }
}
