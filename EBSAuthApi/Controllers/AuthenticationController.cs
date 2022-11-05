using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBSAuthApi.Models;
using EBSAuthApi.Models.Dtos.Requests;
using EBSAuthApi.Models.Dtos.Responses;
using EBSAuthApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EBSAuthApi.Controllers.Authentication
{
    [ApiController]
    [Route("auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpGet("salt")]
        public async Task<IActionResult> GetClientSalt(string email, CancellationToken cancelToken)
        {
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser(UserLogin user, CancellationToken cancelToken)
        {

            AuthResponseDto result = await _authService.LoginUserAsync(user, cancelToken);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterClient(ClientRegister user, CancellationToken cancelToken)
        {
            AuthResponseDto result = await _authService.RegisterClientAsync(user, cancelToken);

            return Ok(result);
        }
    }
}

