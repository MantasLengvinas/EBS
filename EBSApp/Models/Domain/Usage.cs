using System;
namespace EBSApp.Models.Domain
{
    public class Usage
    {
        public bool ToPay { get; set; }
        public int UsageId { get; set; }
        public DateTime OnDate { get; set; }
        public int ElectricityDue { get; set; }
        public decimal AmountDue { get; set; }
        public decimal PaidTariff { get; set; }
    }
}

