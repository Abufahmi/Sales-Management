using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sales.Context.Helpers;
using Sales.Context.Services;
using Sales.Library;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(LibraryService.Admin))]
    public class SettingsController : ControllerBase
    {
        private readonly IUserService userService;

        public SettingsController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        [Route("DeleteUserById/{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            if (id == null) return BadRequest();
            var result = await userService.DeleteUserByIdAsync(id);
            if (result)
            {
                return Ok(result);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetUserById/{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            if (id == null) return BadRequest();
            var user = await userService.GetUserByIdAsync(id);
            if (user != null)
            {
                return Ok(user);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserList()
        {
            var users = await userService.GetUsersAsync();
            if (users != null)
            {
                return Ok(users);
            }

            return NoContent();
        }

        [HttpPost]
        [Route("CreateUser")]
        public async Task<IActionResult> CreateUser(User user)
        {
            if (user == null) return BadRequest();
            bool result = await userService.CreateUserAsync(user);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(User user)
        {
            if (user == null) return BadRequest();
            bool result = await userService.UpdateUserAsync(user);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
