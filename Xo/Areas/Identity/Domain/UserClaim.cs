using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Xo.Areas.Identity.Domain
{
    public class UserClaim
    {
        [Obsolete("Runtime use only.", error: true)]
        public UserClaim() { }
        public UserClaim(Guid userId, string claimType, string claimValue)
        {
            this.UserId = userId;
            this.Type = claimType;
            this.Value = claimValue;
        }

        [Key, Column(Order = 1)]
        [Required]
        public Guid UserId { get; private set; }
        public virtual User User { get; private set; }

        [Key, Column(Order = 2)]
        [Required]
        public string Type { get; private set; }

        [Key, Column(Order = 3)]
        [Required]
        public string Value { get; private set; }

        /// <summary>
        /// Returns this user claim translated to a security claim.
        /// </summary>
        /// <returns></returns>
        public Claim ToSecurityClaim()
        {
            return new Claim(Type, Value);
        }
    }
}
