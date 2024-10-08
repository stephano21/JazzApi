namespace JazzApi.DTOs
{
    public class DeviceDTO
    {
        public string UserId { get; set; }            // Identificador único del dispositivo
        public string UniqueId { get; set; }            // Identificador único del dispositivo
        public string Token { get; set; }
        public string Brand { get; set; }               // Marca del dispositivo (ej. Apple, Samsung)
        public string Model { get; set; }               // Modelo del dispositivo (ej. iPhone 12, Galaxy S21)
        public string SystemName { get; set; }          // Sistema operativo (ej. iOS, Android)
        public string SystemVersion { get; set; }       // Versión del sistema operativo (ej. 14.0, 11.0)
        public float? BatteryLevel { get; set; }        // Nivel de batería (ej. 0.75 para 75%)
        public bool? IsCharging { get; set; }           // Si el dispositivo está cargando
        public bool? IsRooted { get; set; }             // Si el dispositivo está rooteado/jailbreak
        public double? Latitude { get; set; }           // Latitud de la ubicación del dispositivo
        public double? Longitude { get; set; }          // Longitud de la ubicación del dispositivo
        public string LocationPermissionStatus { get; set; }  // Estado del permiso de ubicación
        public string CameraPermissionStatus { get; set; }    // Estado del permiso de cámara
        public string NotificationPermissionStatus { get; set; }  // Estado del permiso de notificaciones
        public string ConnectionType { get; set; }      // Tipo de conexión a Internet (WiFi, celular, etc.)
        public bool? IsConnected { get; set; }
    }
}
