using EBSApp.Models;

namespace EBSApp.Services.General
{
    public interface IApiClient
    {
        Task<ApiResponse<T>> GetAsync<T>(string url, CancellationToken cancelToken = default) where T : class, new();
        Task<ApiResponse<T>> PostAsync<T, R>(string url, R data, CancellationToken cancelToken)
            where T : class, new()
            where R : class, new();
    }
}