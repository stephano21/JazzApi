using Hangfire;
using JazzApi.DTOs.Reto;
using JazzApi.Manager;
using System.Diagnostics;
using System.Net;

namespace JazzApi.Hangfire
{
    public class HangfireService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly object _lockObject = new();
        private bool _started;
        public HangfireService(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        [Obsolete]
        public void Start()
        {
            lock (_lockObject)
            {
                if (_started) return;

                _started = true;

                RecurringJob.RemoveIfExists("Recargar Factura");
                if (Debugger.IsAttached)
                {
                    //RecurringJob.AddOrUpdate("Recargar Página", () => this.CargarSitio(), "*/4 * * * *");
                    RecurringJob.AddOrUpdate("Recargar Factura", () => this.CargarFacturas(), "*/10 * * * *");
                    
                }
            }
        }

        //[DisableMultipleQueuedItemsFilter]
        public async Task CargarFacturas()
        {
            if (Debugger.IsAttached)
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    //var fechaImplementacion = new DateTime(2023, 10, 1); // fecha para prueba
                    var fechaImplementacion = new DateTime(2023, 12, 6);
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var servicio = new TaskManager(dbContext,"HangFire","","hangfire");
                    var Nuevo = new TaskDTO
                    {
                        Title = "Handd",
                        Description = "kosdfisdufv"
                    };
                    await servicio.Save(Nuevo);
                }
            }
        }

    }
}
