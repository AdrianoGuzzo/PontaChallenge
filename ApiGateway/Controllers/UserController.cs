using ApiGateway.Models;
using ApiGateway.Security;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ApiGateway.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly UserService _userService;
        public UserController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("SignIn")]
        public async Task<ActionResult> SignIn([FromForm] string login, [FromForm] string password)
        {
            Token? token = await _userService.GetToken(login, password);
            return token is not null ? Ok(token) : Unauthorized();
        }
    }
}
