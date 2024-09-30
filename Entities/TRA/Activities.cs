using JazzApi.Entities.Auditory;
using JazzApi.Entities.Auth;
using JazzApi.Entities.CAT;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JazzApi.Entities.TRA
{
    [Table("Activities",Schema ="TRA")]
    public class Activity : Audit
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? Expiration { get; set; }
        public string Frecuency {  get; set; }
        public long TypeId { get; set; }
        [ForeignKey("TypeId")]
        public virtual TypeActivity TypeActivity { get; set; }
    }
}
