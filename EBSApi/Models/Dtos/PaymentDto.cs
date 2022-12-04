namespace EBSApi.Models.Dtos
{
    public class PaymentDto
    {
        public IEnumerable<Usage>? Usages { get; set; }
        public double PaymentSum { get; set; }
    }
}
