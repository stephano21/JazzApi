using System.ComponentModel.DataAnnotations.Schema;

namespace JazzApi.Entities.Auth
{
    [Table("Porfile", Schema = "AUTH")]
    public class Porfile
    {
        public long IdPorfile { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
