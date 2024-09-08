using JazzApi.DTOs.Auth;
using JazzApi.Entities.Auth;
using JazzApi.Manager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JazzApi.Controllers.Auth
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationUserManager _userManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AuthController( ApplicationUserManager ApplicationUserManager, 
            IConfiguration IConfiguration,
            IHttpContextAccessor httpContextAccessor)
        {
            this.httpContextAccessor = httpContextAccessor;
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
        [HttpGet("Profile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> Profile() => Ok(await _userManager.GetProfile(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value));
        [HttpGet("Profile/Sync")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> SyncProfile(bool Refesh=false) => Ok(await _userManager.GetOrRegisterSyncCode(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value, Refesh));
        [HttpDelete("Profile/Sync")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> ClearProfile() => Ok(await _userManager.RemoveCouple(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value));
        [HttpPost("Profile/Sync")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> PairCouple(string PairCode) => Ok(await _userManager.SyncCouple(httpContextAccessor.HttpContext.User.Claims
                .FirstOrDefault(c => c.Type == "Username").Value, PairCode));
    }
}
