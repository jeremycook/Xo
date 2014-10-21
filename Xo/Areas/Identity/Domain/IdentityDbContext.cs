using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Xo.Areas.Identity.Domain
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

        public virtual IDbSet<Identity> Identities { get; set; }
        public virtual IDbSet<SystemAccount> SystemAccounts { get; set; }

        public virtual IDbSet<Role> Roles { get; set; }
        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<UserLogin> Logins { get; set; }
        public virtual IDbSet<UserClaim> Claims { get; set; }
    }
}