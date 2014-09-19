using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Xo.Areas.Accounts.Models
{
    public class ApplicationDbContext : DbContext
    {
        static ApplicationDbContext()
        {
            Database.SetInitializer<ApplicationDbContext>(
                new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public virtual IDbSet<ApplicationUser> Users { get; set; }
        public virtual IDbSet<IdentityUserLogin> Logins { get; set; }
    }
}