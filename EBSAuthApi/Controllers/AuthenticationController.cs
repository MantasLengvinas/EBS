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
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public AuthenticationController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost]
        public async Task<ActionResult<GenericResponse<UserJwt>>> AuthenticateUser(UserLogin user, CancellationToken cancelToken)
        {
            UserJwt jwt = await _authService.AuthenticateUser(user, cancelToken);

            if (jwt is null)
                return Forbid();

            GenericResponse<UserJwt> response = new()
            {
                Data = jwt,
            };

            return Ok(jwt);
        }
    }
}

