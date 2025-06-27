using Microsoft.AspNetCore.Mvc;
using Sales.Library.Models;
using Sales.Context.Helpers;
using Microsoft.AspNetCore.Authorization;
using FluentEmail.Core;
using System.Net.Mail;
using FluentEmail.Smtp;
using System.Text;
using FluentEmail.Razor;
using Sales.Context.Contracts;
using Sales.Library.Entities;

namespace Sales.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService accountService;
        private readonly IConfiguration config;
        private readonly IUserService userService;

        public AccountController(IAccountService accountService, IConfiguration config, 
            IUserService userService)
        {
            this.accountService = accountService;
            this.config = config;
            this.userService = userService;
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

        [Authorize(Roles = nameof(LibraryService.Admin))]
        [HttpGet]
        [Route("TestAuthentication")]
        public IActionResult TestAuthentication()
        {
            return Ok("Welcome admin you are authenticated"); ;
        }

        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenModel? refreshToken)
        {
            if (refreshToken?.RefreshToken == null)
                return BadRequest();

            var token = await accountService.RefreshTokenAsync(refreshToken.RefreshToken);
            if (token != null)
            {
                return Ok(token);
            }

            if (LibraryService.Error != null)
                return BadRequest(LibraryService.Error);

            return BadRequest();
        }

        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(ForgetPassword? forgetPassword)
        {
            if (forgetPassword?.EmailAddress == null || forgetPassword?.EmailModel?.Subject == null ||
                forgetPassword?.EmailModel?.Body == null)
                return BadRequest();

            var result = await accountService.ForgetPasswordAsync(forgetPassword.EmailAddress);
            if (result.Flag && result.token != null)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine($"<h3>{forgetPassword.EmailModel.Body}</h3>");
                sb.AppendLine($"<h4>{forgetPassword.EmailModel.VerifyCode}</h4>");
                sb.AppendLine($"<h1>{result.refreshToken}</h1>");
                sb.AppendLine($"<h6>The Sales Team.</h6>");
                    

                var hostEmail = config.GetValue<string>("emailSection:Host");
                var sender = new SmtpSender(() => new SmtpClient(hostEmail)
                {
                    EnableSsl = false,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = config.GetValue<int>("emailSection:MailPort")
                });

                Email.DefaultSender = sender;
                Email.DefaultRenderer = new RazorRenderer();
                var email = await Email
                   .From(config.GetSection("EmailSection:EmailAdrress").Value)
                   .To(forgetPassword.EmailAddress)
                   .Subject(forgetPassword.EmailModel.Subject)
                   .UsingTemplate(sb.ToString(), forgetPassword)
                   .SendAsync();

                if (email.Successful)
                    return Ok(result.token);
            }

            if (result?.message != null)
                return BadRequest(result.message);

            return BadRequest();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("VerifyCode/{verificationCode}")]
        public async Task<IActionResult> VerifyCode(string? verificationCode)
        {
            if (verificationCode == null) return BadRequest();
            var result = await accountService.VerifyCodeAsync(verificationCode);
            if (result != null)
                return Ok(result);

            if (LibraryService.Error != null)
                return BadRequest(LibraryService.Error);

            return BadRequest();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("IsTokenExists/{token}")]
        public async Task<IActionResult> IsTokenExists(string? token)
        {
            if (token == null) return BadRequest();
            var result = await accountService.IsTokenExistsAsync(token);
            if (result.Flag)
                return Ok();

            if (result.message != null)
                return BadRequest(result.message);

            return BadRequest();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost]
        [Route("ResetAccount")]
        public async Task<IActionResult> ResetAccount(ResetAccount? resetAccount)
        {
            if (resetAccount?.UserId == null || resetAccount?.Password == null)
                return BadRequest();

            var tokenModel = await accountService.ResetAccountAsync(resetAccount);
            if (tokenModel)
            {
                return Ok();
            }

            return BadRequest();
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        [Route("GetMainSetting")]
        public async Task<IActionResult> GetMainSetting()
        {
            var mainSetting = await accountService.GetMainSettingAsync();
            if (mainSetting != null)
            {
                return Ok(mainSetting);
            }

            return BadRequest();
        }


        [HttpGet]
        [Route("IsUserExest/{id}")]
        public async Task<IActionResult> IsUserExest(string id)
        {
            bool result = await userService.IsUserExestAsync(id);
            if (result)
            {
                return Ok();
            }
            return BadRequest();
        }
    }
}
