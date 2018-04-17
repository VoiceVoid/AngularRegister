using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace AngularRegister.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
		public string FirstName { get; set; }
		public string LastName { get; set; }
	}

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

		protected override void OnModelCreating(DbModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);
			//AspNetUsers -> User
			modelBuilder.Entity<ApplicationUser>()
				.ToTable("User");
			//AspNetUsers -> User
			modelBuilder.Entity<IdentityRole>()
				.ToTable("Role");
			//AspNetUsers -> User
			modelBuilder.Entity<IdentityUserRole>()
				.ToTable("UserRole");
			//AspNetUsers -> User
			modelBuilder.Entity<IdentityUserClaim>()
				.ToTable("UserClaim");
			//AspNetUsers -> User
			modelBuilder.Entity<IdentityUserLogin>()
				.ToTable("UserLogin");
		}

		public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}