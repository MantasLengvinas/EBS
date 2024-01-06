using System;
using System.Net;

namespace EBSAuthApi.Models.Dtos.Responses
{
    public class AuthResponseDto
    {
        public bool IsSuccess { get; set; } = false;
        public string SessionToken { get; set; }
        public string ErrorMessage { get; set; }
        public HttpStatusCode? StatusCode { get; set; }

        public AuthResponseDto() { }

        public AuthResponseDto(string errorMessage, HttpStatusCode statusCode)
        {
            ErrorMessage = errorMessage;
            StatusCode = statusCode;
        }

        public AuthResponseDto(string sessionToken, bool isSuccess = true)
        {
            SessionToken = sessionToken;
            IsSuccess = isSuccess;
        }
    }
}

