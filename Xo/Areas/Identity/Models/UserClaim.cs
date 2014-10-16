using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Xo.Areas.Identity.Models
{
    public class UserClaim
    {
        [Obsolete("Runtime use only.", error: true)]
        public UserClaim() { }
        public UserClaim(Guid userId, string claimType, string claimValue)
        {
            this.UserId = userId;
            this.ClaimType = claimType;
            this.ClaimValue = claimValue;
        }

        [Key, Column(Order = 1)]
        [Required]
        public Guid UserId { get; private set; }

        [Key, Column(Order = 2)]
        [Required]
        public string ClaimType { get; private set; }

        [Key, Column(Order = 3)]
        [Required]
        public string ClaimValue { get; private set; }

        /// <summary>
        /// Returns this user claim translated to a security claim.
        /// </summary>
        /// <returns></returns>
        public Claim ToSecurityClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }
    }
}
