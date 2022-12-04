using EBSApi.Models;
using EBSApi.Data;
using Microsoft.AspNetCore.Mvc;
using EBSApi.Models.Dtos;

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
            Response<IEnumerable<User>> response = await _userQueries.GetAllUsersAsync();
            if (response.Data == null || !response.Data.Any())
                return NotFound();
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync([FromBody] User user, CancellationToken cancellationToken)
        {
            Response<User> response = await _userQueries.CreateUserAsync(user);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserAsync(int id, CancellationToken cancellationToken)
        {
            Response<User> response = await _userQueries.GetUserAsync(id);
            if (response.Data == null)
                return NotFound();
            return Ok(response);
        }
    }
}
