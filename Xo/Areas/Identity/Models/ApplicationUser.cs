using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Xo.Areas.Identity.Services;

namespace Xo.Areas.Identity.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        [Obsolete("Runtime use only.", error: true)]
        public ApplicationUser()
        {
        }

        /// <summary>
        /// Construct a brand new, not yet in the database, user.
        /// </summary>
        /// <param name="id"></param>
        public ApplicationUser(Guid id)
        {
            this.Id = id;
            this.Logins = new List<IdentityUserLogin>();
            this.LockoutEndDate = DateTimeOffset.UtcNow.AddMinutes(-1);
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

        public virtual ICollection<IdentityUserLogin> Logins { get; private set; }

        //// Lockout

        public int AccessFailedCount { get; set; }

        public bool LockoutEnabled { get; set; }

        public DateTimeOffset LockoutEndDate { get; set; }

        //// Security Stamp

        public string SecurityStamp { get; set; }
    }
}