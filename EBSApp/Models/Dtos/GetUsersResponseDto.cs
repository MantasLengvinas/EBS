using System;
namespace EBSApp.Models.Dtos
{
    public class GetUsersResponseDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public decimal Balance { get; set; }
        public bool Business { get; set; }
    }
}

