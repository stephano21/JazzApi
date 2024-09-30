using JazzApi.Entities.Auditory;
using JazzApi.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JazzApi.Entities.TRA
{
    [Table("Goals",Schema ="TRA")]
    public class Goal: Audit
    {
        [Key]
        public long Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual Profile Profile { get; set; }
    }
}
