using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Xo.Areas.Identity.Domain
{
    public abstract class UserBase : IUser<int>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual UserId Id { get; protected set; }
        public virtual string UserName { get; protected set; }

        int IUser<int>.Id
        {
            get { return (int)Id; }
        }

        string IUser<int>.UserName
        {
            get { return UserName; }
            set { UserName = value; }
        }
    }
}
