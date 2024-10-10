using JazzApi.DTOs;
using JazzApi.DTOs.Auth;
using JazzApi.Entities.Auth;
using JazzApi.Entities.CAT;
using JazzApi.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
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
                    NickName = UserData.Profile.NickName,
                    SyncCode = Guid.NewGuid().ToString("N").Substring(0, 6),
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
            var plantilla = _mailManager.LoadEmailTemplate("ConfirmEmailTemplate");
            plantilla = _mailManager.PopulateTemplate(plantilla, Params);
            var DeliveredMail = await _mailManager.SendEmail(EmailAddress, "Confirm Account", plantilla);
            if (!DeliveredMail.Successful) return false;
            return true;
        }
        public async Task<LoggedUser> LoginUserAsync(LoginDTO credencialesUsuario)
        {
            if (string.IsNullOrEmpty(credencialesUsuario.Username) || string.IsNullOrEmpty(credencialesUsuario.Password))
                throw new Exception("Credenciales Incompletas!");

            var usuario = await FindByNameAsync(credencialesUsuario.Username);
            credencialesUsuario.Device.UserId = usuario.Id;
            if (credencialesUsuario.Device is not null)
                await HandelInfoDevice(credencialesUsuario.Device);
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
            if (user.EmailConfirmed) throw new Exception("Este codigo de verificacion ya fue utilizado!");
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
            var plantilla = _mailManager.LoadEmailTemplate("ConfirmEmailTemplate");
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
            var UserData = await _contex.Profile.Include(x => x.Couple).Where(x => x.UserId.Equals(usuario.Id)).FirstOrDefaultAsync();

            return new ProfileViewDTO
            {
                Email = usuario.Email,
                UserName = Username,
                FirstName = UserData.FirstName,
                LastName = UserData.LastName,
                NickName = UserData.NickName,
                Couple = $"{UserData.Couple?.FirstName ?? ""} {UserData.Couple?.LastName ?? ""}",
            };
        }
        public async Task<string> GetOrRegisterSyncCode(string Username, bool Refresh = false)
        {
            var usuario = await FindByNameAsync(Username);
            var UserData = await _contex.Profile.FindAsync(usuario.Id);
            if (string.IsNullOrEmpty(UserData.SyncCode) || Refresh)
            {
                UserData.SyncCode = Guid.NewGuid().ToString("N").Substring(0, 6);
                await _contex.SaveChangesAsync();
            }
            return UserData.SyncCode;
        }
        public async Task<string> RemoveCouple(string Username)
        {
            var usuario = await FindByNameAsync(Username);
            var UserData = await _contex.Profile.FindAsync(usuario.Id);
            if (!string.IsNullOrEmpty(UserData.SyncCode))
            {
                var Couple = await _contex.Profile.Where(x => x.UserId.Equals(UserData.CoupleId)).FirstOrDefaultAsync();
                UserData.CoupleId = null;
                if (Couple is not null) Couple.CoupleId = null;
                await _contex.SaveChangesAsync();
            }
            return "Se Ha eliminado a su pareja con éxito!";
        }
        public async Task<string> SyncCouple(string Username, string PairCode)
        {
            if (string.IsNullOrEmpty(PairCode) || (PairCode.Count() < 6 || PairCode.Count() > 6)) throw new Exception("Ingrese un codigo Valifo");
            var usuario = await FindByNameAsync(Username);
            var Couple = await _contex.Profile.Include(x => x.User).Where(x => x.SyncCode.Equals(PairCode)).FirstOrDefaultAsync();
            var UserData = await _contex.Profile.FindAsync(usuario.Id);
            if (Couple is null) throw new Exception("Invalid Pair Code");
            if (UserData.SyncCode.Equals(PairCode)) throw new Exception("Your self Invalid Pair Code");
            if (!string.IsNullOrEmpty(UserData.CoupleId)) throw new Exception("Ya estás emparejado con un usuario, elimina tu pareja actual para continuar!");
            if (!string.IsNullOrEmpty(Couple.CoupleId)) throw new Exception("El usuario ya está emparejado!");
            UserData.CoupleId = Couple.UserId;
            Couple.CoupleId = UserData.UserId;
            await _contex.SaveChangesAsync();

            var transmitter = new Dictionary<string, string>
            {
                { "title", $"Emparejamiento exitoso" },
                { "message", $"{Couple.FirstName}, {UserData.FullName()} se ha emparejado contigo" },
            };
            var Sender = new Dictionary<string, string>
            {
                { "title", $"Emparejamiento exitoso" },
                { "message", $"Te has emparejado exitosamente con {Couple.FullName()}" },
            };
            await SendNotifyAsync(usuario.Email, Sender);
            await SendNotifyAsync(Couple.User.Email, transmitter);
            return "Se ha emparejado con exito!";
        }
        public async Task SendNotifyAsync(string Email, Dictionary<string, string> Content)
        {
            var EmailAddress = new List<string>
                    {
                        Email
                    };
            var plantilla = _mailManager.LoadEmailTemplate("PairCoupleTemplate");
            plantilla = _mailManager.PopulateTemplate(plantilla, Content);
            var DeliveredMail = await _mailManager.SendEmail(EmailAddress, "Couple Succes", plantilla);
        }
        public async Task HandelInfoDevice(DeviceDTO data)
        {
            var ExisteDevice = await _contex.Device.Where(x => x.UniqueId.Equals(data.UniqueId)).FirstOrDefaultAsync();
            if (ExisteDevice is not null)
            {
                var NuevoDevice = new Device
                {
                    IdDevice = Guid.NewGuid(),
                    UniqueId = data.UniqueId,
                    Token = data.Token,
                    Brand = data.Brand,
                    Model = data.Model,
                    SystemName = data.SystemName,
                    SystemVersion = data.SystemVersion,
                    BatteryLevel = data.BatteryLevel,
                    IsCharging = data.IsCharging,
                    IsRooted = data.IsRooted,
                    LocationPermissionStatus = data.LocationPermissionStatus,
                    CameraPermissionStatus = data.CameraPermissionStatus,
                    NotificationPermissionStatus = data.NotificationPermissionStatus,
                    ConnectionType = data.ConnectionType,
                    IsConnected = data.IsConnected,
                    UserId = data.UserId,
                };
                //NuevoDevice.Register("","");
                await _contex.Device.AddAsync(NuevoDevice);
            }
            else
            {
                ExisteDevice.Token = data.Token;
                ExisteDevice.Brand = data.Brand;
                ExisteDevice.Model = data.Model;
                ExisteDevice.SystemName = data.SystemName;
                ExisteDevice.SystemVersion = data.SystemVersion;
                ExisteDevice.BatteryLevel = data.BatteryLevel;
                ExisteDevice.IsCharging = data.IsCharging;
                ExisteDevice.IsRooted = data.IsRooted;
                ExisteDevice.LocationPermissionStatus = data.LocationPermissionStatus;
                ExisteDevice.CameraPermissionStatus = data.CameraPermissionStatus;
                ExisteDevice.NotificationPermissionStatus = data.NotificationPermissionStatus;
                ExisteDevice.ConnectionType = data.ConnectionType;
                ExisteDevice.IsConnected = data.IsConnected;
            }
            await _contex.SaveChangesAsync();
        }
    }
}
