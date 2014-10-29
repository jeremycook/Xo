using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Xo.Areas.Users.Domain;

namespace Xo.Areas.Users.Security
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params RoleId[] roleIds)
        {
            Roles = string.Join(",", roleIds.Select(o => o.ToString()));
        }
    }
}
