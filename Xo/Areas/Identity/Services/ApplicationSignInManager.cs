using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Xo.Areas.Identity.Models;

namespace Xo.Areas.Identity.Services
{
    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<User, Guid>
    {
        /// <remarks>
        /// Private because I don't want to accidentally use this instead of the Create method.
        /// </remarks>
        private ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
        }

        public override Guid ConvertIdFromString(string id)
        {
            return string.IsNullOrEmpty(id) ? Guid.Empty : Guid.Parse(id);
        }
    }
}