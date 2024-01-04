namespace EBSAuthApi.Models.Dtos.Requests
{
    public interface IUserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
