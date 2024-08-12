using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sales.Context.Contracts;
using Sales.Context.Helpers;
using Sales.Library;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = nameof(LibraryService.Admin))]
    public class SettingsController : ControllerBase
    {
        private readonly IUserService userService;
        private readonly IUserRoleService userRoleService;
        private readonly IMainSettingService mainSettingService;

        public SettingsController(IUserService userService, IUserRoleService userRoleService, 
            IMainSettingService mainSettingService)
        {
            this.userService = userService;
            this.userRoleService = userRoleService;
            this.mainSettingService = mainSettingService;
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

        [HttpGet]
        [Route("GetUserRoles")]
        public async Task<IActionResult> GetUserRoles()
        {
            var userRoles = await userRoleService.GetUserRolesAsync();
            if (userRoles != null)
            {
                return Ok(userRoles);
            }

            return NoContent();
        }

        [HttpDelete]
        [Route("DeleteUserRoleById/{id}")]
        public async Task<IActionResult> DeleteUserRoleById(string id)
        {
            if (id == null) return BadRequest();
            bool result = await userRoleService.DeleteUserRoleByIdAsync(id);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetUserRoleById/{id}")]
        public async Task<IActionResult> GetUserRoleById(string id)
        {
            if (id == null) return BadRequest();
            UserRole? userRole = await userRoleService.GetUserRoleByIdAsync(id);
            if (userRole != null)
            {
                return Ok(userRole);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var roles = await userRoleService.GetRolesAsync();
            if (roles != null)
            {
                return Ok(roles);
            }

            return NoContent();
        }

        [HttpGet]
        [Route("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await userService.GetUsersAsync();
            if (users != null)
            {
                return Ok(users);
            }

            return NoContent();
        }

        [HttpPut]
        [Route("UpdateUserRole")]
        public async Task<IActionResult> UpdateUserRole(UserRole userRole)
        {
            if (userRole == null) return BadRequest();
            bool result = await userRoleService.UpdateUserRoleAsync(userRole);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("CreateUserRole")]
        public async Task<IActionResult> CreateUserRole(UserRole userRole)
        {
            if (userRole == null) return BadRequest();
            bool result = await userRoleService.CreateUserRoleAsync(userRole);
            if (result)
            {
                return Ok();
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("GetMainSettingById/{id}")]
        public async Task<IActionResult> GetMainSettingById(int id)
        {
            if (id == 0) return BadRequest();
            MainSetting? main = await mainSettingService.GetMainSettingByIdAsync(id);
            if (main != null)
            {
                return Ok(main);
            }

            return NotFound();
        }
        
        [HttpGet]
        [Route("GetMainSetting")]
        public async Task<IActionResult> GetMainSetting()
        {
            MainSetting? main = await mainSettingService.GetMainSettingAsync();
            if (main != null)
            {
                return Ok(main);
            }

            return NotFound();
        }

        [HttpPost]
        [Route("CreateMainSetting")]
        public async Task<IActionResult> CreateMainSetting(MainSetting mainSetting)
        {
            if (mainSetting == null) return BadRequest();
            MainSetting? main = await mainSettingService.CreateMainSettingAsync(mainSetting);
            if (main != null)
            {
                return Ok(main);
            }
            else
            {
                if (LibraryService.Error != null)
                {
                    return BadRequest(LibraryService.Error);
                }
            }

            return NotFound();
        }
        
        [HttpPut]
        [Route("UpdateMainSetting")]
        public async Task<IActionResult> UpdateMainSetting(MainSetting mainSetting)
        {
            if (mainSetting == null) return BadRequest();
            MainSetting? main = await mainSettingService.UpdateMainSettingAsync(mainSetting);
            if (main != null)
            {
                return Ok(main);
            }
            else
            {
                if (LibraryService.Error != null)
                {
                    return BadRequest(LibraryService.Error);
                }
            }

            return NotFound();
        }
    }
}
