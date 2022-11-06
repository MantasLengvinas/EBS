using System;
namespace EBSAuthApi.Models.Domain
{
    public class UserInfo
    {
        public int ClientId { get; set; }
        public int Id { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        public bool Active { get; set; }
        public bool Business { get; set; }
        public bool Completed { get; set; }
    }
}

