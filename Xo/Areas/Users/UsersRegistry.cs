using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Xo.Areas.Users.Domain;
using Xo.Areas.Users.Services;

namespace Xo.Areas.Infrastructure
{
    public class UsersRegistry : Registry
    {
        public UsersRegistry()
        {
            // CODESMELL Although currently working, it makes me nervous
            // because the managers get their identity db context from OWIN and
            // not this container.
            For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);
            For<UsersDbContext>().Use(() => HttpContext.Current.GetOwinContext().Get<UsersDbContext>());
            For<ApplicationUserManager>().Use(() => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
            For<ApplicationRoleManager>().Use(() => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationRoleManager>());
            For<ApplicationSignInManager>().Use(() => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>());
        }
    }
}
