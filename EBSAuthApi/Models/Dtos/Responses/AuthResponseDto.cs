using System;
namespace EBSAuthApi.Models.Dtos.Responses
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; } = false;
        public string? SessionToken { get; set; }
        public string? ErrorMessage { get; set; }
    }
}

