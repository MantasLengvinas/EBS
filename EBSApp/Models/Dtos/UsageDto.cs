using System;
namespace EBSApp.Models.Dtos
{
    public class UsageDto
    {
        public DateTime OnDate { get; set; }
        public decimal ElectricityDue { get; set; }
        public bool IsPaid { get; set; }
        public int AddressId { get; set; }
        public int PaidTariff { get; set; }
    }
}

