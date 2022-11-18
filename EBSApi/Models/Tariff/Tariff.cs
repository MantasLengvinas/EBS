namespace EBSApi.Models
{
    public class Tariff
    {
        public int TariffId { get; set; }
        public DateTime? RegistryDate { get; set; }
        public double Rate { get; set; }
        public int ProviderId { get; set; }
        public bool IsBusiness { get; set; }
    }
}
