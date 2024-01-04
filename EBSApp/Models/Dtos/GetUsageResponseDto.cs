using System;
using EBSApp.Models.Domain;

namespace EBSApp.Models.Dtos
{
    public class GetUsageResponseDto
    {
        public List<Usage> Usages { get; set; } = new();
        public decimal PaymentSum { get; set; }
    }
}

