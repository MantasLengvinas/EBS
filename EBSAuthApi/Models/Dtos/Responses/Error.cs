using System;
namespace EBSAuthApi.Models.Dtos.Responses
{
    public class Error
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
    }
}

