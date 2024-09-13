using LibrarySystem.Domain;
using LibrarySystem.Domain.Interfaces;
using LibrarySystem.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySystem.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromForm] RegisterModel model, [FromForm] IFormFileCollection attachments)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            model.Attachments = attachments;

            var result = await _authService.RegisterAsync(model);

            if (result.Status == "Error")
                return BadRequest(result.Message);

            
            var mailData = new MailData
            {
                EmailToIds = new List<string> { model.Email },
                EmailToName = model.UserName,
                EmailSubject = "Welcome to Our Service!",
                EmailBody = "Thank you for registering with us.",
                Attachments = attachments
            };

            var emailResult = await _emailService.SendMail(mailData, model);
            if (!emailResult)
            {
                return Ok(new ResponseModel
                {
                    Status = "Success",
                    Message = "Registration successful but failed to send the welcome email."
                });
            }

            return Ok(result);
        }


        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _authService.LoginAsync(model);

            if (result.Status == "Error")
                return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("role")]
        public async Task<IActionResult> CreateRoleAsync([FromBody] string rolename)
        {
            var result = await _authService.CreateRoleAsync(rolename);
            if (result.Status == "Error")
            {
                return BadRequest(result.Message);
            }
            return Ok(result);
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestModel model)
        {
            var response = await _authService.RefreshTokenAsync(model);
            if (response.Status == "Success")
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
    }
}
