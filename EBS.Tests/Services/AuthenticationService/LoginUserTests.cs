using EBSAuthApi.Data;
using EBSAuthApi.Models.Dtos.Requests;
using EBSAuthApi.Options;
using EBSAuthApi.Services;
using EBSAuthApi.Utility;
using FluentAssertions;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EBS.Tests.Services.AuthenticationService
{
    public class LoginUserTests
    {
        private readonly IAuthenticationService _authService;
        private readonly IOptions<AuthenticationOptions> _options;

        public LoginUserTests()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.EBSAuthApi.json")
                .Build();

            string connectionString = config.GetConnectionString("EBSAuth");
            SqlUtility sqlUtility = new(connectionString);

            IAuthenticationQueries queries = new AuthenticationQueries(sqlUtility);

            string jwtSigningKey = config.GetValue<string>("Jwt:Key");
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPrivateKey(Convert.FromBase64String(jwtSigningKey), out _);

            IJwtGenerator jwtGenerator = new JwtGenerator(new RsaSecurityKey(rsa));

            AuthenticationOptions options = new()
            {
                Audience = "EBS_tests",
                Issuer = "EBS",
                ExpirationTimeInSeconds = 60,
            };

            _options = Options.Create(options);

            _authService = new EBSAuthApi.Services.AuthenticationService(_options, jwtGenerator, queries);
        }

        [Fact]
        public async Task LoginUser_ValidCredentials_ReturnsSessionToken()
          {
            UserLoginDto credentials = new()
            {
                Email = "mantaslengvin@gmail.com",
                Password = "bWFudGFz"
            };

            var result = await _authService.LoginUserAsync(credentials, default);

            Assert.NotNull(result);
            result.IsSuccess.Should().BeTrue();
            result.ErrorMessage.Should().BeNull();
            result.SessionToken.Should().NotBeNull();
        }

        [Fact]
        public async Task LoginUser_MissingCredentials_ReturnsErrorMessage()
        {
            UserLoginDto credentials = new()
            {
                Email = "mantaslengvingmail.com",
                Password = ""
            };

            var result = await _authService.LoginUserAsync(credentials, default);

            Assert.NotNull(result);
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNull();
            result.SessionToken.Should().BeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task LoginUser_NotExistingUser_ReturnsErrorMessage()
        {
            UserLoginDto credentials = new()
            {
                Email = "mantaslengvingmail.com",
                Password = "bWFudGFz"
            };

            var result = await _authService.LoginUserAsync(credentials, default);

            Assert.NotNull(result);
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNull();
            result.SessionToken.Should().BeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task LoginUser_IncorrectPassword_ReturnsErrorMessage()
        {
            UserLoginDto credentials = new()
            {
                Email = "mantaslengvin@gmail.com",
                Password = "bWFudGE="
            };

            var result = await _authService.LoginUserAsync(credentials, default);

            Assert.NotNull(result);
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().NotBeNull();
            result.SessionToken.Should().BeNull();
            result.StatusCode.Should().Be(System.Net.HttpStatusCode.BadRequest);
        }
    }
}
