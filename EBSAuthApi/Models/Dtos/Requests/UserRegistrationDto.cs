using System;
namespace EBSAuthApi.Models.Dtos.Requests
{
    public class UserRegistrationDto : IUserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

