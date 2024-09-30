using JazzApi.Entities.Auditory;
using JazzApi.Entities.TRA;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JazzApi.Entities.CAT
{
    [Table("TypesActivity", Schema ="CAT")]
    public class TypeActivity: Audit
    {
        [Key]
        public long Id { get; set; }
        public string Type { get; set; }
        public virtual ICollection<Activity> Activities { get; set; }
    }
}
