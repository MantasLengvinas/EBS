namespace EBSApi.Models.Dtos
{
    public class Response<T>
    {
        public T? Data { get; set; }
        public bool IsSuccess { get; set; } = false;
        public Error? Error { get; set; }
    }
}
