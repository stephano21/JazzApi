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

        public AuthController( ApplicationUserManager ApplicationUserManager, IConfiguration IConfiguration)
        {
            _userManager = ApplicationUserManager;
        }
        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync(LoginDTO data) => Ok(await _userManager.LoginUserAsync(data));
        [HttpPost("register")]
        public async Task<ActionResult> RegisterAsync(RegisterDTO UserData) => Ok(await _userManager.RegisterUserAsync(UserData));
        
    }
}
