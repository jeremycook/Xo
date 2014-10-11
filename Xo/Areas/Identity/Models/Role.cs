using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Xo.Areas.Identity.Models
{
    public class Role : IRole<Guid>
    {
        [Obsolete("Runtime use only.", error: true)]
        public Role() { }

        public Role(string name, string description = null)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Description = description;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual ICollection<UserRole> Users { get; private set; }
    }
}