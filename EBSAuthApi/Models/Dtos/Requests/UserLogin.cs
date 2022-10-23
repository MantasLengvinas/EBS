using System;
namespace EBSAuthApi.Models.Dtos.Requests
{
    public class UserLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

