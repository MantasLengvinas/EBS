using EBSAuthApi.Models.Dtos.Requests;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using EBSAuthApi;
using Newtonsoft.Json;
using EBSAuthApi.Models.Dtos.Responses;

namespace EBS.Tests.Endpoints.Authentication
{
    public class AuthenticateUserTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        private static readonly string _requestUrl = "/auth/login";

        public AuthenticateUserTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        public static readonly UserLoginDto _correctCredentials = new()
        {
            Email = "mantaslengvin@gmail.com",
            Password = "bWFudGFz"
        };

        public static readonly UserLoginDto _notExistingUser = new()
        {
            Email = "fancyEmail@gmail.com",
            Password = "bWFudGFz"
        };

        public static readonly UserLoginDto _incorrectCredentials = new()
        {
            Email = "mantaslengvin@gmail.com",
            Password = "bWFudGE="
        };

        [Fact]
        public async Task LoginUser_ShouldReturnSessionToken()
        {
            var response = await _client.PostAsJsonAsync(_requestUrl, _correctCredentials, default);

            var stringContent = await response.Content.ReadAsStringAsync();
            var authResponse = JsonConvert.DeserializeObject<AuthResponseDto>(stringContent);

            response.EnsureSuccessStatusCode();
            authResponse.SessionToken.Should().NotBeNull();
            authResponse.ErrorMessage.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task LoginUser_ShouldReturnNotFoundStatusCode()
        {
            var response = await _client.PostAsJsonAsync(_requestUrl, _notExistingUser, default);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LoginUser_ShouldReturnBadRequestCode()
        {
            var response = await _client.PostAsJsonAsync(_requestUrl, _incorrectCredentials, default);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
