using System;
namespace EBSAuthApi.Models.Dtos.Requests
{
    public class CompleteRegistration
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; } = 5000;
        public bool Business { get; set; }
    }
}

