using System;
using System.Net;
using EBSApp.Models.Dtos.Responses;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace EBSApp.Services.General
{
    public class ApiClient : IApiClient
    {
        private readonly HttpClient _client;

        public ApiClient(HttpClient httpClient)
        {
            _client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<GenericResponse<T>> GetAsync<T>(string url, CancellationToken cancelToken) where T : class, new()
        {
            GenericResponse<T> response = new() { IsSuccess = false };
            HttpStatusCode statusCode = HttpStatusCode.Processing;

            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(url, cancelToken);

                statusCode = responseMessage.StatusCode;
                responseMessage.EnsureSuccessStatusCode();

                string content = await responseMessage.Content.ReadAsStringAsync(cancelToken);

                T result;
                result = JsonConvert.DeserializeObject<T>(content);

                response.Data = result;
                response.IsSuccess = true;

                return response;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Error = new Error()
                {
                    StatusCode = (int)statusCode,
                    Message = "Unhandled exception"
                };

                return response;
            }
        }

        public async Task<GenericResponse<T>> PostAsync<T, R>(string url, R data, CancellationToken cancelToken)
        {
            GenericResponse<T> response = new() { IsSuccess = false };
            HttpStatusCode statusCode = HttpStatusCode.Processing;

            try
            {
                JsonContent requestContent = JsonContent.Create(data);

                HttpResponseMessage responseMessage = await _client.PostAsync(url, requestContent, cancelToken);

                statusCode = responseMessage.StatusCode;
                responseMessage.EnsureSuccessStatusCode();

                string content = await responseMessage.Content.ReadAsStringAsync(cancelToken);

                T? result;
                result = JsonConvert.DeserializeObject<T>(content);

                response.Data = result;
                response.IsSuccess = true;

                return response;
            }
            catch (Exception e)
            {
                response.IsSuccess = false;
                response.Error = new Error()
                {
                    StatusCode = (int)statusCode,
                    Message = e.Message
                };

                return response;
            }
        }
    }
}

