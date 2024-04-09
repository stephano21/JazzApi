using System.ComponentModel.DataAnnotations;

namespace JazzApi.DTOs.Auth
{
    public class AuthDTO
    {
    }
    public class LoginDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class RegisterDTO:UserDTO
    {
        [Required]
        public string Password { get; set; }
    }
    public class UserDTO
    {
        [Required]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }
        public string Role { get; set; }="User";
        public PorfileDTO Porfile { get; set; }
    }
    public class PorfileDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
    }
    public class LoggedUser
    {
        public TokensDTO Auth { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public DateTime Expiracion { get; set; }
        public string Env { get; set; }
    }
    public class TokensDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
