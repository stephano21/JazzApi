using Hangfire;
using JazzApi;
using JazzApi.Hangfire;
Console.WriteLine("Inicio Program");
var builder = WebApplication.CreateBuilder(args);
Console.WriteLine("Inicio llamado al star up");
var startup = new Startup(builder.Configuration);
Console.WriteLine("fin llamado al star up");
startup.ConfigureServices(builder.Services);
Console.WriteLine("Configurando servicios");
var app = builder.Build();
Console.WriteLine("Construyendo servicios");
var backgroundJobClient = app.Services.GetRequiredService<IBackgroundJobClient>();
var hangfireService = (HangfireService)app.Services.GetService(typeof(HangfireService));
startup.Configure(app, app.Environment, backgroundJobClient, hangfireService);
Console.WriteLine("Fin Program");

app.Run();
