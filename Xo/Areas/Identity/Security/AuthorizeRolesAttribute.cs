using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Xo.Areas.Identity.Domain;

namespace Xo.Areas.Identity.Security
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        public AuthorizeRolesAttribute(params RoleId[] roleIds)
        {
            Roles = string.Join(",", roleIds.Select(o => o.ToString()));
        }
    }
}
