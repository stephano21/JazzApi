namespace JazzApi.DTOs.Auth
{
    public class AuthDTO
    {
    }
    public class LoginDTO
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    public class RegisterDTO:UserDTO
    {
        public string Password { get; set; }
    }
    public class UserDTO
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
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
        public string Token { get; set; }
        public string Rol { get; set; }
        public DateTime Expiracion { get; set; }
        public string Ambiente { get; set; }
    }
}
