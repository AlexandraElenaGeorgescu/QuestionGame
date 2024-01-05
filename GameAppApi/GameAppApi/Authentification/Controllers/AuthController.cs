using GameAppApi.API.PublicModels;
using GameAppApi.Authentification.Models;
using GameAppApi.Authentification.PublicServices;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GameAppApi.Authentification.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User registration)
        {
            var userExists = _userService.GetUserByUsername(registration.Username);
            if (userExists != null)
            {
                return BadRequest("User already exists.");
            }

            registration.Role = registration.Role;
            registration.Id = Guid.NewGuid();


            var createdUser = await _userService.CreateUser(registration);
            if (createdUser == null)
            {
                return BadRequest("User could not be created.");
            }

            return Ok(new { userId = createdUser.Id });
        }


        [HttpPost("login")]
        public IActionResult Login([FromBody] User loginUser)
        {
            var user = _userService.GetUserByUsername(loginUser.Username);
            if (user == null || loginUser.Password != user.Password)
            {
                return Unauthorized("User does not exist or password is incorrect.");
            }

            if (user.Role == Role.Moderator)
            {
                return Ok(new { message = "Logged in as moderator", user.Role, userData = user });
            }
            else if (user.Role == Role.Player)
            {
                return Ok(new { message = "Logged in as player", user.Role, userData = user });
            }

            return BadRequest("Role not recognized.");
        }


    }

}
