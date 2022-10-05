using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EBSApi.Models.Authentication;
using EBSApi.Services.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EBSApi.Controllers.Authentication
{
    [ApiController]
    [Route("api/auth")]
    public class TokensController : ControllerBase
    {
        private readonly IAuthenticationService _authService;

        public TokensController(IAuthenticationService authenticationService)
        {
            _authService = authenticationService ?? throw new ArgumentNullException(nameof(authenticationService));
        }

        [HttpPost]
        public IActionResult GetJwt(User user, CancellationToken cancelToken)
        {
            string jwt = _authService.GetJwt(user, cancelToken);
            return Ok(jwt);
        }
    }
}

