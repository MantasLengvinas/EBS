using System;
using System.Net;
using EBSApp.Models;
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

        public async Task<ApiResponse<T>> GetAsync<T>(string url, CancellationToken cancelToken) where T : class, new()
        {
            ApiResponse<T> apiResponse = new() { IsSuccess = false };
            GenericResponse<T> genericResponse = new();
            HttpStatusCode statusCode = HttpStatusCode.Processing;

            try
            {
                HttpResponseMessage responseMessage = await _client.GetAsync(url, cancelToken);

                statusCode = responseMessage.StatusCode;
                responseMessage.EnsureSuccessStatusCode();

                string content = await responseMessage.Content.ReadAsStringAsync(cancelToken);

                if (!string.IsNullOrEmpty(content))
                {
                    genericResponse = JsonConvert.DeserializeObject<GenericResponse<T>>(content);
                    if(genericResponse != null)
                        apiResponse.Data = genericResponse.Data;
                }
                

                apiResponse.IsSuccess = true;

                return apiResponse;
            }
            catch (Exception e)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Error = new Error()
                {
                    StatusCode = (int)statusCode,
                    Message = "Unhandled exception"
                };

                return apiResponse;
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T, R>(string url, R data, CancellationToken cancelToken)
            where T : class, new()
            where R : class, new()
        {
            ApiResponse<T> apiResponse = new();
            GenericResponse<T> genericResponse = new();
            HttpStatusCode statusCode = HttpStatusCode.Processing;

            try
            {
                JsonContent requestContent = JsonContent.Create(data);

                HttpResponseMessage responseMessage = await _client.PostAsync(url, requestContent, cancelToken);

                statusCode = responseMessage.StatusCode;
                responseMessage.EnsureSuccessStatusCode();

                string content = await responseMessage.Content.ReadAsStringAsync(cancelToken);

                if (!string.IsNullOrEmpty(content))
                {
                    genericResponse = JsonConvert.DeserializeObject<GenericResponse<T>>(content);
                    if (genericResponse != null)
                        apiResponse.Data = genericResponse.Data;
                }

                apiResponse.IsSuccess = true;

                return apiResponse;
            }
            catch (Exception e)
            {
                apiResponse.IsSuccess = false;
                apiResponse.Error = new Error()
                {
                    StatusCode = (int)statusCode,
                    Message = e.Message
                };

                return apiResponse;
            }
        }

        public async Task PutAsync(string url, CancellationToken cancelToken)
        {
            HttpStatusCode statusCode = HttpStatusCode.Processing;

            try
            {

                HttpResponseMessage responseMessage = await _client.PutAsync(url, null, cancelToken);

                statusCode = responseMessage.StatusCode;
                responseMessage.EnsureSuccessStatusCode();


            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public async Task<bool> DeleteAsync(string url, CancellationToken cancelToken)
        {
            try
            {

                HttpResponseMessage responseMessage = await _client.DeleteAsync(url, cancelToken);

                responseMessage.EnsureSuccessStatusCode();
                return true;

            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}

