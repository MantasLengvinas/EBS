namespace EBSApi.Models.Dtos
{
    public class Rate
    {
        public double RateAmbiguous { get; set; }
        public double RateDay { get; set; }
        public double RateEvening { get; set; }
        public double RateNight { get; set; }
        public DateTime OnDate { get; set; }
    }
}
