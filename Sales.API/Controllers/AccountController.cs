using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using Sales.Context.Models;
using Sales.Context.Services;

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

            return BadRequest(result);
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

            return BadRequest(result);
        }
    }
}
