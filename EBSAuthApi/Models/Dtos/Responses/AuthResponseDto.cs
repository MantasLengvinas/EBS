using System;
namespace EBSAuthApi.Models.Dtos.Responses
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; } = false;
        public string SessionToken { get; set; }
        public string ErrorMessage { get; set; }

        public AuthResponseDto() { }

        public AuthResponseDto(string errorMessage)
        {
            ErrorMessage = errorMessage;
        }

        public AuthResponseDto(string sessionToken, bool isSuccess)
        {
            SessionToken = sessionToken;
            IsSuccess = isSuccess;
        }
    }
}

