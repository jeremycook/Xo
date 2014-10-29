using Microsoft.AspNet.Identity;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Xo.Areas.Users.Domain
{
    public class UsersDbContext : DbContext
    {
        static UsersDbContext()
        {
            Database.SetInitializer<UsersDbContext>(null);
        }

        public static UsersDbContext Create()
        {
            return new UsersDbContext();
        }

        public UsersDbContext()
            : base("DefaultConnection")
        {
        }

        public virtual IDbSet<User> Users { get; set; }
        public virtual IDbSet<SystemAccount> SystemAccounts { get; set; }

        public virtual IDbSet<Role> Roles { get; set; }
        public virtual IDbSet<UserAccount> UserAccounts { get; set; }
        public virtual IDbSet<UserLogin> Logins { get; set; }
        public virtual IDbSet<UserClaim> Claims { get; set; }
    }
}