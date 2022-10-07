﻿using System;
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

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> AuthenticateUser(UserLogin user, CancellationToken cancelToken)
        {

            string jwt = await _authService.AuthenticateUser(user, cancelToken);

            if (jwt is null)
                return Unauthorized();

            return Ok(
                new AuthResponseDto
                {
                    SessionToken = jwt,
                    Status = "success"
                });
        }
    }
}

