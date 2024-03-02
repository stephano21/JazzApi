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
        public virtual ApplicationUser User { get; set; }
    }
}
