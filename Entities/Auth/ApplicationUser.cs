using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JazzApi.Entities.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public bool PasswordExpired { get; set; }= false;
        public bool Lock { get; set; }= false;
        [Required]
        public Profile Profile { get; set; }

    }
}
