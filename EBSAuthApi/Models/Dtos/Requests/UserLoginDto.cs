﻿using System;
namespace EBSAuthApi.Models.Dtos.Requests
{
    public class UserLoginDto : IUserCredentials
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}

