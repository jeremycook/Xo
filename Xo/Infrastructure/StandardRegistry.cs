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
using Xo.Areas.Identity.Models;
using Xo.Areas.Identity.Services;

namespace Xo.Infrastructure
{
    public class StandardRegistry : Registry
    {
        public StandardRegistry()
        {
            Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            });

            // CODESMELL
            For<IAuthenticationManager>().Use(() => HttpContext.Current.GetOwinContext().Authentication);
            For<ApplicationUserManager>().Use(() => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
            For<ApplicationSignInManager>().Use(() => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>());
        }
    }
}
