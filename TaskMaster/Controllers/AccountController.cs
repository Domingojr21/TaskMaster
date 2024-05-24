using Microsoft.AspNetCore.Mvc;
using TaskMaster.Core.Application.Dtos.Account;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;
using TaskMaster.Infraestructure.Identity.Services;
using TaskMaster.Core.Application.FluentValidations;

namespace TaskMaster.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Session Start/Session Maintenance")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Login")]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "Authentication",
            Description = "Here you can log in with an existing user"
        )]
        public async Task<IActionResult> AuthenticateAsync([FromBody] AuthenticationRequest request)
        {
            var account = await _accountService.AuthenticateAsync(request, true);

            return Ok(account);
        }

        [HttpPost("Register")]
        [Consumes(MediaTypeNames.Application.Json)]
        [SwaggerOperation(
            Summary = "User Registration",
            Description = "Here you can register or create a user for this service"
        )]
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterRequest request)
        {
            var validator = new RegisterRequestValidator();
            var validationResult = await validator.ValidateAsync(request);

            if (!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
                return BadRequest(new { code = 400, error = errors });
            }
            var origin = Request?.Headers["origin"].ToString() ?? string.Empty;

            return Ok(await _accountService.RegisterUserAsync(request, origin, "Developer"));
        }

    }
}
