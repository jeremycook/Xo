using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Xo.Areas.Identity.Domain
{
    public class Role : IRole<int>
    {
        [Obsolete("Runtime use only.", error: true)]
        public Role() { }

        public Role(RoleId id, string name)
        {
            this.Id = id;
            this.Name = name;
            this.Users = new List<UserRole>();
        }

        public RoleId Id { get; private set; }

        [Required]
        public string Name { get; private set; }
        public string Description { get; set; }
        public virtual ICollection<UserRole> Users { get; private set; }

        int IRole<int>.Id
        {
            get { return (int)Id; }
        }

        string IRole<int>.Name
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}