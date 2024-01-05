using GameAppApi;
using GameAppApi.API.PublicModels;
using GameAppApi.Authentification.PublicServices;
using GameAppApi.UserAdministration.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameAppApi.UserAdministration.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly AdminService _adminService;

        public UserController(AdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            var users = await _adminService.GetAllUsers();
            return Ok(users);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _adminService.GetUserById(id); 
            if (user == null)
            {
                return NotFound();
            }

            await _adminService.Remove(user.Id.ToString());
            return NoContent();
        }

    }
}
