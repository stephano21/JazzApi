using JazzApi.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JazzApi.Entities.Reto
{
    [Table("TaskNotes", Schema = "CAT")]
    public class TaskNotes
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Profile")]
        public string UserId { get; set; }
        public virtual Profile Profile { get; set; }
    }
}
