using System;
namespace EBSAuthApi.Models.Dtos.Responses
{
    public class GenericResponse<T> where T : class, new()
    {
        public GenericResponse() {}

        public GenericResponse(T data)
        {
            Data = data;
        }

        public GenericResponse(Error error)
        {
            Error = error;
        }

        public GenericResponse(T data, Error error)
        {
            Data = data;
            Error = error;
        }

        public T Data { get; set; }
        public Error Error { get; set; }
        public bool IsSuccess { get; set; }
    }
}

