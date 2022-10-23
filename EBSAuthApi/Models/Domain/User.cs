using System;
using System.Security.Claims;

namespace EBSAuthApi.Models.Domain
{
    public class User
    {
        public UserInfo UserInfo { get; set; }
        public List<Claim> UserClaims { get; set; }
    }
}

