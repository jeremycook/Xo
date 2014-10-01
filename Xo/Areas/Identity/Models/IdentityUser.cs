using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xo.Areas.Identity.Models
{
    public abstract class IdentityUser : IUser<Guid>
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
    }
}
