using JazzApi.DTOs.Auth;
using JazzApi.Entities.Auth;
using JazzApi.Helpers;
using JazzApi.Templates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;

namespace JazzApi.Manager
{
    public class ApplicationUserManager : UserManager<ApplicationUser>
    {
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IServiceProvider services;
        private readonly IConfiguration configuration;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Services.MailService.MailService.MailRepository _mailManager;
        private readonly ApplicationDbContext _contex;
        public ApplicationUserManager(IHttpContextAccessor httpContextAccessor,
           IConfiguration IConfiguration,
           SignInManager<ApplicationUser> signInManager,
           IUserStore<ApplicationUser> store,
           IOptions<IdentityOptions> optionsAccessor,
           IPasswordHasher<ApplicationUser> passwordHasher,
           IEnumerable<IUserValidator<ApplicationUser>> userValidators,
           IEnumerable<IPasswordValidator<ApplicationUser>> passwordValidators,
           ILookupNormalizer keyNormalizer,
           IdentityErrorDescriber errors,
           IServiceProvider services,
           ILogger<UserManager<ApplicationUser>> logger,
           RoleManager<IdentityRole> roleManager,
            ApplicationDbContext Context) :
           base(store,
               optionsAccessor,
               passwordHasher,
               userValidators,
               passwordValidators,
               keyNormalizer,
               errors,
               services,
               logger)
        {
            _userStore = store;
            _roleManager = roleManager;
            configuration = IConfiguration;
            this.services = services;
            this.signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
            _mailManager = new Services.MailService.MailService.MailRepository(configuration);
            _contex = Context;

        }
        public async Task<bool> RegisterUserAsync(RegisterDTO UserData)
        {
            UserData.UserName = UserData.UserName.Trim();
            UserData.Email = UserData.Email.Trim();
            UserData.Profile.LastName = UserData.Profile.LastName.Trim();
            UserData.Profile.FirstName = UserData.Profile.FirstName.Trim();
            if (await Users.Where(u => u.Email == UserData.Email).AnyAsync()) throw new Exception("El correo ingresado ya se está utilizando");
            if (await Users.Where(u => u.UserName == UserData.UserName).AnyAsync()) throw new Exception("El nombre de usuario debe ser unico");
            ApplicationUser usuario = new ApplicationUser
            {
                UserName = UserData.UserName,
                Email = UserData.Email,
                Profile = new Profile
                {
                    FirstName = UserData.Profile.FirstName,
                    LastName = UserData.Profile.LastName,
                    NickName = UserData.Profile.NickName
                }

            };

            if (!(await CreateAsync(usuario, UserData.Password)).Succeeded) return false;
            // Verificar si el rol existe, y si no existe, crearlo
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            string roleName = UserData.Role; // Reemplaza con el nombre del rol deseado
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }

            // Asignar al usuario al rol deseado
            var result = await AddToRoleAsync(usuario, roleName);
            Claim roleClaim = new Claim("Rol", roleName);
            result = await AddClaimAsync(usuario, roleClaim);

            if (!result.Succeeded) return false;
            var EmailAddress = new List<string>
                    {
                        usuario.Email,
                    };
            var code = await GenerateEmailConfirmationTokenAsync(usuario);
            var callbackUrl = await GenerateConfirmationUrlAsync(usuario);
            var Params = new Dictionary<string, string>
            {
                { "title", $"Confirmacion de correo para {usuario.UserName}" },
                { "url", callbackUrl },
                { "message", "Confirma tu direccion de correo electronico en el sigueinte enlace!" },
            };
            var plantilla = _mailManager.LoadEmailTemplate();
            plantilla = _mailManager.PopulateTemplate(plantilla, Params);
            var DeliveredMail = await _mailManager.SendEmail(EmailAddress, "Confirm Account", plantilla);
            if (!DeliveredMail.Successful) return false;
            return true;
        }
        public async Task<LoggedUser> LoginUserAsync(LoginDTO credencialesUsuario)
        {
            if (string.IsNullOrEmpty(credencialesUsuario.Username) || string.IsNullOrEmpty(credencialesUsuario.Password)) throw new Exception("Credenciales Incompletas!");
            var usuario = await FindByNameAsync(credencialesUsuario.Username);
            if (usuario != null && !usuario.Lock)
            {
                var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Username, credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: true);

                if (resultado.Succeeded)
                {
                    return await ConstruirTokenv2(credencialesUsuario);
                }
                if (resultado.IsLockedOut) throw new Exception("Cuenta bloqueada temporalmente");
                if (resultado.IsNotAllowed) throw new Exception("Valide su correo para luego iniciar sesion!");
            }

            throw new Exception("Credenciales Inválidas");
        }

        public async Task<string> GetRole(ApplicationUser user)
        {
            try
            {
                // Obtenemos el rol del usuario usando el UserManager
                IList<string> userRoles = await GetRolesAsync(user);

                // Suponemos que el usuario tiene un único rol asignado
                if (userRoles.Count == 1)
                {
                    // Obtenemos el primer y único rol del usuario
                    string userRole = userRoles.First();
                    return userRole;
                }
                else
                {
                    // Si el usuario no tiene roles o tiene más de uno, retornamos un valor por defecto o manejas el caso según tu lógica de negocio.
                    return "DefaultRole"; // Por ejemplo, puedes retornar un valor por defecto.
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<string> GenerateConfirmationUrlAsync(ApplicationUser user)
        {
            var SITE = Environment.GetEnvironmentVariable("SITE") ?? configuration["SITE"];
            var code = await GenerateEmailConfirmationTokenAsync(user);
            var encryptionHelper = new EncryptionHelper(configuration);
            var encryptedUserId = encryptionHelper.Encrypt(user.Id);
            var encryptedCode = encryptionHelper.Encrypt(code);
            var baseUrl = "";
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                baseUrl = $"{SITE}";
            }
            else
            {
                baseUrl = $"{SITE}";
            }
            return $"{baseUrl}/email/confirm?userId={WebUtility.UrlEncode(encryptedUserId)}&code={WebUtility.UrlEncode(encryptedCode)}";
        }
        public async Task<bool> ConfirmEmail(string userIdEncript, string codeEncript)
        {
            var encryptionHelper = new EncryptionHelper(configuration);
            var userId = encryptionHelper.Decrypt(userIdEncript);
            var code = encryptionHelper.Decrypt(codeEncript);
            var user = await FindByIdAsync(userId);
            if (user == null) throw new Exception("Usuario no encontrado");
            var result = await ConfirmEmailAsync(user, code);
            if (result.Succeeded) return true;
            throw new Exception($"Error al confirmar el correo");
        }
        public async Task<bool> ResendEmailConfirmation(string Email)
        {
            var EmailAddress = new List<string>
                    {
                        Email,
                    };
            var usuario = await FindByEmailAsync(Email);
            if (usuario == null) throw new Exception("Correo no registrado");
            var callbackUrl = await GenerateConfirmationUrlAsync(usuario);
            var Params = new Dictionary<string, string>
            {
                { "title", $"Confirmacion de correo para {usuario.UserName}" },
                { "url", callbackUrl },
                { "message", "Confirma tu direccion de correo electronico en el sigueinte enlace!" },
            };
            var plantilla = _mailManager.LoadEmailTemplate();
            plantilla = _mailManager.PopulateTemplate(plantilla, Params);
            var DeliveredMail = await _mailManager.SendEmail(EmailAddress, "Confirm Account", plantilla);
            if (!DeliveredMail.Successful) return false;
            return true;
        }
        private async Task<LoggedUser> ConstruirTokenv2(LoginDTO credencialesUsuario)
        {
            var usuario = await FindByNameAsync(credencialesUsuario.Username);
            var Profile = await _contex.Profile.FindAsync(usuario.Id);
            var claims = new List<Claim>()
            {
                new Claim("Username", credencialesUsuario.Username),
            };

            // Obtener roles del usuario
            var roles = await GetRolesAsync(usuario);
            foreach (var rol in roles)
            {
                claims.Add(new Claim("Role", rol));
            }

            var llaveAcceso = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTKey") ?? configuration["JWTKey"]));
            var llaveActualizacion = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTRefreshKey") ?? configuration["JWTRefreshKey"]));
            var credsAcceso = new SigningCredentials(llaveAcceso, SecurityAlgorithms.HmacSha256);
            var credsActualizacion = new SigningCredentials(llaveActualizacion, SecurityAlgorithms.HmacSha256);

            var expiracion = DateTime.UtcNow.AddDays(1);

            var accessToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: credsAcceso);
            var refreshToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: DateTime.UtcNow.AddDays(7), signingCredentials: credsActualizacion);

            return new LoggedUser()
            {
                Auth = new TokensDTO
                {
                    Access_Token = new JwtSecurityTokenHandler().WriteToken(accessToken),
                    Refresh_Token = new JwtSecurityTokenHandler().WriteToken(refreshToken)
                },
                Username = credencialesUsuario.Username,
                FullName = $"{Profile?.FirstName} {Profile?.LastName}",
                Role = await GetRole(usuario),
                Expiracion = expiracion,
                Env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? configuration["Env"]
            };
        }
        public async Task<ProfileViewDTO> GetProfile(string Username)
        {
            var usuario = await FindByNameAsync(Username);
            var UserData = await _contex.Profile.FindAsync(usuario.Id);

            return new ProfileViewDTO
            {
                Email = usuario.Email,
                UserName = Username,
                FirstName = UserData.FirstName,
                LastName = UserData.LastName,
                NickName = UserData.NickName,
                Couple= UserData.Couple?.LastName??"",
            };
        }
    }
}
