using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Xo.Areas.Identity.Models
{
    public class UserRole
    {
        [Obsolete("Runtime use only.", error: true)]
        public UserRole() { }

        public UserRole(Models.Role role)
        {
            this.Role = role;
        }

        [Key, Column(Order = 1)]
        public Guid UserId { get; set; }
        public virtual User User { get; private set; }

        [Key, Column(Order = 2)]
        public Guid RoleId { get; set; }
        public virtual Role Role { get; private set; }
    }
}
