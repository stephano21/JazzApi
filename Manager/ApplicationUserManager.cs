﻿using JazzApi.DTOs.Auth;
using JazzApi.Entities.Auth;
using JazzApi.Templates;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
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
           RoleManager<IdentityRole> roleManager) :
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

        }
        public async Task<bool> RegisterUserAsync(RegisterDTO UserData)
        {
            UserData.UserName = UserData.UserName.Trim();
            UserData.Email = UserData.Email.Trim();
            UserData.Porfile.LastName = UserData.Porfile.LastName.Trim();
            UserData.Porfile.FirstName = UserData.Porfile.FirstName.Trim();
            if (await Users.Where(u => u.Email == UserData.Email).AnyAsync()) throw new Exception("El correo ingresado ya se está utilizando");
            if (await Users.Where(u => u.UserName == UserData.UserName).AnyAsync()) throw new Exception("El nombre de usuario debe ser unico");
            ApplicationUser usuario = new ApplicationUser
            {
                UserName = UserData.UserName,
                Email = UserData.Email,
                Profile = new Profile
                {
                    FirstName = UserData.Porfile.FirstName,
                    LastName = UserData.Porfile.LastName,
                    NickName = UserData.Porfile.NickName
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
            var plantilla = ConfirmEmailTemplate.Template($"Confirmacion de correo para {usuario.UserName}", callbackUrl,
                "Confirma tu direccion de correo electronico en el sigueinte enlace!");
            var DeliveredMail = await _mailManager.SendEmail(EmailAddress, "Confirm Account", plantilla);
            if(!DeliveredMail.Successful) return false;
            return true;
        }

        public async Task<LoggedUser> LoginUserAsync(LoginDTO credencialesUsuario)
        {
            if( string.IsNullOrEmpty(credencialesUsuario.Username) || string.IsNullOrEmpty(credencialesUsuario.Password)) throw new Exception("Credenciales Incompletas!");
            var usuario = await FindByNameAsync(credencialesUsuario.Username);
            if (usuario != null && !usuario.Lock)
            {
                var resultado = await signInManager.PasswordSignInAsync(credencialesUsuario.Username, credencialesUsuario.Password, isPersistent: false, lockoutOnFailure: true);

                if (resultado.Succeeded)
                {
                    return await ConstruirToken(credencialesUsuario);
                }
                if (resultado.IsLockedOut) throw new Exception("Cuenta bloqueada temporalmente");
                if (resultado.IsNotAllowed) throw new Exception("Valide su correo para luego iniciarsesion!");
            }

            throw new Exception("Credenciales Inválidas");
        }
        private async Task<string> GenerateConfirmationUrlAsync(ApplicationUser user)
        {
            var code = await GenerateEmailConfirmationTokenAsync(user);

            // Construct the confirmation URL manually
            var baseUrl = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}";
            return $"{baseUrl}/api/auth/ConfirmEmail?userId={user.Id}&code={code}";
        }
        private async Task<LoggedUser> ConstruirToken(LoginDTO credencialesUsuario)
        {
            var usuario = await FindByNameAsync(credencialesUsuario.Username);
            //realmente el claim seria username
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

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTKey"]));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(1);

            var securtityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new LoggedUser()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securtityToken),
                Rol = await GetRole(usuario),
                Expiracion = expiracion,
                Ambiente = configuration["Env"]
            };
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
        public async Task<bool> ConfirmEmail(string userId, string code)
        {
            var user = await FindByIdAsync(userId);
            if (user == null) throw new Exception("Usuario no encontrado");
            var result = await ConfirmEmailAsync(user, code);
            if (result.Succeeded) return true;
            throw new Exception($"Error al confirmar el correo");
        }   
    }
}
