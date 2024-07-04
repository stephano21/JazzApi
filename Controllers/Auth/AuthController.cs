using JazzApi.DTOs.Auth;
using JazzApi.Entities.Auth;
using JazzApi.Manager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JazzApi.Controllers.Auth
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationUserManager _userManager;

        public AuthController( ApplicationUserManager ApplicationUserManager, 
            IConfiguration IConfiguration,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = ApplicationUserManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody]LoginDTO data) => Ok(await _userManager.LoginUserAsync(data));
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync([FromBody] RegisterDTO UserData) => Ok(await _userManager.RegisterUserAsync(UserData));
        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)=> Ok(await _userManager.ConfirmEmail(userId, code));
        [HttpPost("ConfirmEmail")]
        public async Task<IActionResult> ForwardConfirmEmail(string Email) => Ok(await _userManager.ResendEmailConfirmation(Email));
    }
}
