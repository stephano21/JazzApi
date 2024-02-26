using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JazzApi.Controllers.Auth
{
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        [HttpPost("login")]
        public ActionResult Login()
        {
            return Ok();
        }
        [HttpPost("register")]
        public ActionResult Register()
        {
            return Ok();
        }
    }
}
