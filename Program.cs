using Hangfire;
using JazzApi;
using JazzApi.Hangfire;
Console.WriteLine("Inicio Program");
var builder = WebApplication.CreateBuilder(args);
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);
var app = builder.Build();
var backgroundJobClient = app.Services.GetRequiredService<IBackgroundJobClient>();
var hangfireService = (HangfireService)app.Services.GetService(typeof(HangfireService));
startup.Configure(app, app.Environment, backgroundJobClient, hangfireService);
Console.WriteLine("Fin Program");

app.Run();
