namespace EBSApi.Models
{
    public class Usage
    {
        public DateTime? OnDate { get; set; }
        public double ElectricityDue { get; set; }
        public bool IsPaid { get; set; }
        public int AddressId { get; set; }
    }
}
