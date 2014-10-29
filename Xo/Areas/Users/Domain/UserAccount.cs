using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Xo.Areas.Users.Services;

namespace Xo.Areas.Users.Domain
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class UserAccount : User
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Obsolete("Runtime use only.", error: true)]
        public UserAccount()
        {
        }

        /// <summary>
        /// Construct a brand new, not yet in the database, user.
        /// </summary>
        /// <param name="id"></param>
        public UserAccount(string userName)
        {
            this.Id = Guid.NewGuid();
            this.UserName = userName;
            this.LockoutEndDate = DateTimeOffset.UtcNow.AddMinutes(-1);
            this.Logins = new List<UserLogin>();
            this.Roles = new List<UserRole>();
            this.Claims = new List<UserClaim>();
        }

        //// Password

        public string PasswordHash { get; set; }

        //// Mobile Phone

        public string PhoneNumber { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        //// Email

        public string Email { get; set; }

        public bool EmailConfirmed { get; set; }

        //// Two Factor

        public bool TwoFactorEnabled { get; set; }

        //// External Logins

        public virtual ICollection<UserLogin> Logins { get; private set; }

        //// Lockout

        public int AccessFailedCount { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset LockoutEndDate { get; set; }

        //// Security Stamp

        public string SecurityStamp { get; set; }

        //// User Roles

        public virtual ICollection<UserRole> Roles { get; private set; }

        //// User Claims

        public virtual ICollection<UserClaim> Claims { get; private set; }
    }
}