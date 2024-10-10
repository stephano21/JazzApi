using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;

namespace JazzApi.Services.Notify
{
    public class FirebaseService
    {
        public FirebaseService(IConfiguration configuration)
        {
            // Inicializar la app de Firebase si no está ya inicializada
            if (FirebaseApp.DefaultInstance == null)
            {
                var Credencial = configuration["FireBaseCredenciales"] ?? Environment.GetEnvironmentVariable("FireBaseCredenciales");
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = GoogleCredential.FromJson(Credencial)
                });
            }
        }
        public async Task SendNotificationAsync(string token, string title, string body)
        {
            var message = new Message()
            {
                Token = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body,
                },
            };

            // Enviar el mensaje a través de Firebase Cloud Messaging
            string response = await FirebaseMessaging.DefaultInstance.SendAsync(message);
            Console.WriteLine("Notificación enviada con éxito: " + response);
        }
    }
}
