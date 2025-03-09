namespace JazzApi.DTOs.TRA
{
    public class ActivityDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Expiration { get; set; }
        public string Frecuency { get; set; }
        public long TypeId { get; set; }
    }
}
