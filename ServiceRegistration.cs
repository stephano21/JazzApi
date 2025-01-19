using JazzApi.Interfaces;
using JazzApi.Manager;
using static JazzApi.Services.MailService.MailService;

namespace JazzApi
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterBusinessServices(this IServiceCollection services)
        {
            {
                services.AddScoped<IMailRepository, MailRepository>();
                services.AddScoped<IActivities, ActivitiesManager>();
                return services;
            }
        }
    }
}
