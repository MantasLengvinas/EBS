using System;
namespace EBSApp.Models.Dtos.Responses
{
    public class GenericResponse<T> where T : class, new()
    {
        public T Data { get; set; }
        public Error Error { get; set; }
    }
}

