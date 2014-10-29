using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Xo.Areas.Users.Domain;

namespace Xo.Areas.Users
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// Return the user id using the UserIdClaimType as a Guid.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Guid GetUserId(this ClaimsIdentity identity)
        {
            return Guid.Parse(Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(identity));
        }

        /// <summary>
        /// Return the user id using the UserIdClaimType as a Guid.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static Guid GetUserId(this IIdentity identity)
        {
            return Guid.Parse(Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(identity));
        }

        /// <summary>
        /// Returns true if there is a TwoFactorRememberBrowser cookie for a user.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<bool> TwoFactorBrowserRememberedAsync(this IAuthenticationManager manager, Guid userId)
        {
            return await Microsoft.Owin.Security.AuthenticationManagerExtensions.TwoFactorBrowserRememberedAsync(
                manager,
                userId.ToString());
        }

        /// <summary>
        /// Extracts login info out of an external identity.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="xsrfKey"></param>
        /// <param name="expectedValue"></param>
        /// <returns></returns>
        public static async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(this IAuthenticationManager manager, string xsrfKey, Guid expectedValue)
        {
            return await Microsoft.Owin.Security.AuthenticationManagerExtensions.GetExternalLoginInfoAsync(manager, xsrfKey, expectedValue.ToString());
        }
    }
}