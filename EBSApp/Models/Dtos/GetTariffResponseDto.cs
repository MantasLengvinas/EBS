using System;
namespace EBSApp.Models.Dtos
{
    public class GetTariffResponseDto
    {
        public int TariffId { get; set; }
        public decimal Rate { get; set; }
        public bool IsBussiness { get; set; }
    }
}

