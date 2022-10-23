using EBSApp.Models.Dtos.Responses;

namespace EBSApp.Services.General
{
    public interface IApiClient
    {
        Task<GenericResponse<T>> GetAsync<T>(string url, CancellationToken cancelToken) where T : class, new();
        Task<GenericResponse<T>> PostAsync<T, R>(string url, R data, CancellationToken cancelToken);
    }
}