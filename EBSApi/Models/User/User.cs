namespace EBSApi.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string? FullName { get; set; }
        public bool Type { get; set; }
        public double Balance { get; set; }
        public bool Active { get; set; }
    }
}
