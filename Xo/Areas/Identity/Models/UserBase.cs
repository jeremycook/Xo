using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xo.Areas.Identity.Models
{
    public abstract class UserBase : IUser<Guid>
    {
        public virtual Guid Id { get; set; }
        public virtual string UserName { get; set; }
    }
}
