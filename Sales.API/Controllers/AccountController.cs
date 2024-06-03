using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Sales.Library.Models;
using Sales.Context.Services;
using Sales.Context.Helpers;
using Sales.Library;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;

        public AccountController(IAccountService accountService)
        {
            this.accountService = accountService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(Register register)
        {
            if (register == null)
                return BadRequest("Model is empty");

            var result = await accountService.RegisterAsync(register);
            if (result.Flag)
                return Ok(result);

            return BadRequest(result.message);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(Login login)
        {
            if (login == null)
                return BadRequest("Model is empty");

            var result = await accountService.LoginAsync(login);
            if (result != null && result.Flag)
            {
                return Ok(result);
            }

            return BadRequest(result?.message);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetUsers")]
        public async Task<ActionResult<List<User>>> GetUsers()
        {
            var users = await accountService.GetUsersAsync();
            if (users != null)
            {
                return Ok(users);
            }

            if (LibraryService.Error != null)
                return BadRequest(LibraryService.Error);

            return BadRequest();
        }
    }
}
