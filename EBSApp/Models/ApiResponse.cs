using System;
using EBSApp.Models.Dtos.Responses;

namespace EBSApp.Models
{
    public class ApiResponse<T> where T : class, new()
    {
        public ApiResponse() { }

        public ApiResponse(T data)
        {
            Data = data;
        }

        public ApiResponse(Error error)
        {
            Error = error;
        }

        public ApiResponse(T data, Error error)
        {
            Data = data;
            Error = error;
        }

        public T Data { get; set; }
        public Error Error { get; set; }
        public bool IsSuccess { get; set; }
    }
}

