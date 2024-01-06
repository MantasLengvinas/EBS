using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        [HttpPost("login")]
        public async Task<IActionResult> AuthenticateUser(UserLoginDto user, CancellationToken cancelToken)
        {
            AuthResponseDto result = await _authService.LoginUserAsync(user, cancelToken);

            if (result.IsSuccess)
                return Ok(result);
            if (result.StatusCode == HttpStatusCode.NotFound)
                return NotFound(result);
            if (result.StatusCode == HttpStatusCode.BadRequest)
                return BadRequest(result);
            else return StatusCode((int)HttpStatusCode.InternalServerError, result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterClient(UserRegistrationDto user, CancellationToken cancelToken)
        {
            AuthResponseDto result = await _authService.RegisterUserAsync(user, cancelToken);

            return Ok(result);
        }

        [HttpPost("complete")]
        public async Task<IActionResult> CompleteRegistration(CompleteRegistration userInfo, CancellationToken cancelToken)
        {
            bool result = await _authService.CompleteRegistrationAsync(userInfo, cancelToken);

            if (result == false)
                return BadRequest();

            return Ok();
        }
    }
}

