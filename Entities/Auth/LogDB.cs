using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace JazzApi.Entities.Auth
{
    [Table("Log", Schema = "AUTH")]
    public class LogDB
    {
        [Key]
        public long IdLog { get; set; }
        public string RequestID { get; set; }
        public string RequestTraceIdentifier { get; set; }
        public DateTime Fecha { get; set; }
        public string Controller { get; set; }
        public string Endpoint { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }
        public string InnerException { get; set; }
        public string Plataform { get; set; }
        public string Usuario { get; set; }
        public string Ambiente { get; set; }
    }
}
