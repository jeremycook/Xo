using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Xo.Areas.Identity.Domain
{
    public abstract class Identity : IUser<Guid>
    {
        public virtual Guid Id { get; protected set; }
        public virtual string UserName { get; protected set; }

        Guid IUser<Guid>.Id
        {
            get { return Id; }
        }

        string IUser<Guid>.UserName
        {
            get { return UserName; }
            set { UserName = value; }
        }
    }
}
