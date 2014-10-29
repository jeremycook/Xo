using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xo.Areas.Users.Domain;

namespace Xo.Areas.Users.Models
{
    public interface ICurrentUser
    {
        UserAccount User { get; }
    }
}
