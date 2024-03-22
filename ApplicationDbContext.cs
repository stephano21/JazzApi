using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using JazzApi.Entities.Auth;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using JazzApi.Entities.Reto;

namespace JazzApi
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
        public virtual DbSet<LogDB> Log { get; set; }
        public virtual DbSet<Profile> Profile { get; set; }
        public virtual DbSet<TaskNotes> TaskNotes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
           
            modelBuilder.Entity<ApplicationUser>()
            .ToTable("Users", "AUTH");

            modelBuilder.Entity<IdentityUserToken<string>>().ToTable("UsersToken", "AUTH");
            modelBuilder.Entity<IdentityRole>().ToTable("Role", "AUTH");
            modelBuilder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaim", "AUTH");
            modelBuilder.Entity<IdentityUserRole<string>>().ToTable("UserRole", "AUTH");
            modelBuilder.Entity<IdentityUserClaim<string>>().ToTable("UserClaim", "AUTH");
            modelBuilder.Entity<IdentityUserLogin<string>>().ToTable("UserLogin", "AUTH");

           

            modelBuilder.Entity<LogDB>()
                .Property(l => l.Plataform)
                .HasDefaultValue("API");
        }
    }
}
