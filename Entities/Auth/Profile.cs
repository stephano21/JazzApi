using JazzApi.Entities.Reto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JazzApi.Entities.Auth
{
    [Table("Profile", Schema = "AUTH")]
    public class Profile
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string SyncCode { get; set; } = string.Empty;
        [ForeignKey("Couple")]
        public string CoupleId { get; set; }
        // Auto-relación para la pareja
        public virtual Profile Couple { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<TaskNotes> Tasks { get; set; }
        public string FullName() => $"{this.FirstName} {this.LastName}";
    }   
}
