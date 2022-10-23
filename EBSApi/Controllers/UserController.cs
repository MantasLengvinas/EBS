using EBSApi.Models;
using EBSApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EBSApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(CancellationToken cancellationToken)
        {
            IEnumerable<User> users = await _userService.GetAllUsers();
            if (users == null || !users.Any())
                return NotFound();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id, CancellationToken cancellationToken)
        {
            User user = await _userService.GetUser(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
