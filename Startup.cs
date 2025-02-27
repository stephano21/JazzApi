﻿using Hangfire;
using JazzApi.Entities.Auth;
using JazzApi.Filters;
using JazzApi.Hangfire;
using JazzApi.Manager;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using Hangfire.PostgreSql;
namespace JazzApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            Configuration = configuration;
        }
        public IConfiguration Configuration { get; }
        public void ConfigureServices(IServiceCollection services)
        {
            try
            {
                services.RegisterBusinessServices();
                // Configure Filters
                services.AddControllers(opciones =>
                {
                    //Log para captar todos los exeptions no capturados
                    opciones.Filters.Add(typeof(FilterExeption));
                }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles).AddNewtonsoftJson();
                services.AddControllers();
                services.AddEndpointsApiExplorer();
                services.AddSwaggerGen();
                string projectName = Assembly.GetEntryAssembly().GetName().Name;//GET PROYECT NAME
                                                                                //Configure Logger
                Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                    .WriteTo.File($"logs/{projectName}-.txt", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                    .CreateLogger();
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddSerilog(Log.Logger);
                });
                // AUTENTICACIÓN JWT
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).
                    AddJwtBearer(opciones => opciones.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWTKey") ?? Configuration["JWTKey"])),
                        ClockSkew = TimeSpan.Zero
                    });
                Console.WriteLine("JWT -> Ready");
                //HttpContextAccessor
                services.AddHttpContextAccessor();
                //DataProtectionTokenProviderOptions
                services.Configure<DataProtectionTokenProviderOptions>(options =>
                {
                    options.TokenLifespan = TimeSpan.FromHours(24); // o el tiempo que desees
                });

                // IDENTITY (USER SYSTEM)
                services.AddScoped<IdentityDbContext<ApplicationUser>, ApplicationDbContext>();
                services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                })
                    .AddEntityFrameworkStores<ApplicationDbContext>()
                    .AddDefaultTokenProviders();
                Console.WriteLine("IDENTITY (USER SYSTEM) -> Ready");
                //Security on login
                services.AddScoped<ApplicationUserManager>();
                services.AddScoped<SignInManager<ApplicationUser>>();
                services.Configure<IdentityOptions>(options =>
                {
                    options.Lockout.AllowedForNewUsers = true;
                    options.Lockout.MaxFailedAccessAttempts = 3; // Número de intentos fallidos permitidos antes de bloquear la cuenta
                    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(60); // Duración del bloqueo de cuenta
                });
                //SWAGGER
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "JAZZ API",
                        Version = "v1",
                        Description = "API RESTful For the App",
                        Contact = new OpenApiContact
                        {
                            Email = "soporte@jazz.com",
                            Name = "Jazz Soft",
                            Url = new Uri("https://jazz.com")
                        },
                    });

                    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header
                    });

                    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{}
                    }
                });

                    var archivoXML = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                    var rutaXML = Path.Combine(AppContext.BaseDirectory, archivoXML);
                    c.IncludeXmlComments(rutaXML);
                });
                //Configure Database
                var connectionString = Configuration.GetConnectionString("DefaultConnection") ?? "";
                Console.WriteLine($"ConnectionString: {connectionString}");
                var url = Environment.GetEnvironmentVariable("DATABASE_URL");
                Console.WriteLine($"ConnectionString2: {url}");
                Console.WriteLine($"Ambiente: {Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}");
                if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT").Equals("Development"))
                {
                    Console.WriteLine("dev");

                    services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));
                }
                else
                {
                    Console.WriteLine("prod");
                    services.AddDbContext<ApplicationDbContext>(options =>
                   options.UseNpgsql(Environment.GetEnvironmentVariable("DATABASE_URL")));
                    Console.WriteLine("prod");
                }
                Console.WriteLine("Pasando la conexion a la bd");
                services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
                Console.WriteLine("IActionContextAccessor");
                //CORS
                services.AddCors(opciones =>
                {
                    opciones.AddDefaultPolicy(policy =>
                    {
                        policy.WithOrigins("https://www.apirequest.io").AllowAnyMethod().AllowAnyHeader();
                    });
                });
                Console.WriteLine("CORS");
                services.AddSignalR();
                Console.WriteLine("AddSignalR");
                //HANGFIRE
                services.AddHangfire(config =>
                                     config.UsePostgreSqlStorage(c =>
                                         c.UseNpgsqlConnection(Configuration.GetConnectionString("HangfireConnection"))));
                services.AddHangfireServer();
                Console.WriteLine("AddHangfireServer");
                services.AddSingleton<HangfireService>();
                Console.WriteLine("Final start up");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Se calló" + ex.Message);
                Console.WriteLine("Se calló" + ex.InnerException);
                Log.Error(ex, "Error en ConfigureServices");
            }


        }

        [Obsolete]
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IBackgroundJobClient backgroundJobs, HangfireService hangfireService)
        {
            Console.WriteLine("Configurando servicios");
            using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {

                Console.WriteLine("Antes de aplicar migraciones ");
                var dbContext = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations success");
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseHangfireDashboard();
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            hangfireService.Start();
        }
    }
}
