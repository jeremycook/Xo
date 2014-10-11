using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Xo.Areas.Identity.Models
{
    public class IdentityDbContext : DbContext
    {
        static IdentityDbContext()
        {
            Database.SetInitializer<IdentityDbContext>(
                new DropCreateDatabaseIfModelChanges<IdentityDbContext>());
        }

        public static IdentityDbContext Create()
        {
            return new IdentityDbContext();
        }

        public IdentityDbContext()
            : base("DefaultConnection")
        {
        }

        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<IdentityUserLogin> Logins { get; set; }
    }
}