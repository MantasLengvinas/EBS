using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;

namespace EBSApi.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserQueries _userQueries;

        public UserController(IUserQueries userQueries)
        {
            _userQueries = userQueries;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsersAsync(CancellationToken cancellationToken)
        {
            IEnumerable<User> users = await _userQueries.GetAllUsersAsync();
            if (users == null || !users.Any())
                return NotFound();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            User user = await _userQueries.GetUserAsync(id);
            if (user == null)
                return NotFound();
            return Ok(user);
        }
    }
}
