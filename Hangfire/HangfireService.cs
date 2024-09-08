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

                RecurringJob.RemoveIfExists("Recargar Página");
                RecurringJob.RemoveIfExists("Recargar Factura");
                RecurringJob.RemoveIfExists("Actualizar Producto Swiss");
                RecurringJob.RemoveIfExists("Actualizar Producto Swiss Inactivos");
                RecurringJob.RemoveIfExists("Actualizar Catalogo Swiss");
                RecurringJob.RemoveIfExists("Eliminar logs");
                RecurringJob.RemoveIfExists("Sincronizar Solicitudes TMS");
                RecurringJob.RemoveIfExists("Sincronizar Solicitudes Bloqueadas TMS");
                RecurringJob.RemoveIfExists("Verificar Vencimiento Remuneración Variable");
                RecurringJob.RemoveIfExists("Recargar Factura");

               

                if (Debugger.IsAttached)
                {
                    //RecurringJob.AddOrUpdate("Recargar Página", () => this.CargarSitio(), "*/4 * * * *");
                    RecurringJob.AddOrUpdate("Recargar Factura", () => this.CargarFacturas(), "*/10 * * * *");
                    
                }
            }
        }

        public long CargarSitio()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();

            //var AppName = MyConfig.GetValue<string>("urldomain");

            var systemDomain = MyConfig.GetValue<string>("urldomain") ?? "https://conautecqa.apptelink.com/";

            var wr = WebRequest.Create(systemDomain);
            var sw = new Stopwatch();

            sw.Start();
            wr.Timeout = 3500;

            try
            {
                HttpWebResponse response = (HttpWebResponse)wr.GetResponse();
                sw.Stop();
            }
            catch (Exception ex)
            {
                sw.Stop();
                //We know its going to fail but that dosent matter!!
            }
            return sw.ElapsedMilliseconds;
        }
        [AutomaticRetry(Attempts = 0)]
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
