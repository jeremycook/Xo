using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Xo.Areas.Identity.Models
{
    public class Role : IRole<Guid>
    {
        [Obsolete("Runtime use only.", error: true)]
        public Role() { }

        public Role(string name)
        {
            this.Id = Guid.NewGuid();
            this.Name = name;
            this.Users = new List<UserRole>();
        }

        public Guid Id { get; private set; }

        [Required]
        public string Name { get; private set; }
        public string Description { get; set; }
        public virtual ICollection<UserRole> Users { get; private set; }

        Guid IRole<Guid>.Id
        {
            get { return Id; }
        }

        string IRole<Guid>.Name
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}