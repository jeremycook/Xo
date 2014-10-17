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
using Xo.Areas.Identity.Domain;

namespace Xo.Areas.Identity
{
    public static class IdentityExtensions
    {
        /// <summary>
        /// Return the user id using the UserIdClaimType as a Guid.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static int GetUserId(this ClaimsIdentity identity)
        {
            return ((IIdentity)identity).GetUserId();
        }

        /// <summary>
        /// Return the user id using the UserIdClaimType as a Guid.
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        public static int GetUserId(this IIdentity identity)
        {
            int id;
            if (int.TryParse(Microsoft.AspNet.Identity.IdentityExtensions.GetUserId(identity), out id))
            {
                return id;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// Returns true if there is a TwoFactorRememberBrowser cookie for a user.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static async Task<bool> TwoFactorBrowserRememberedAsync(this IAuthenticationManager manager, int userId)
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
        public static async Task<ExternalLoginInfo> GetExternalLoginInfoAsync(this IAuthenticationManager manager, string xsrfKey, int expectedValue)
        {
            return await Microsoft.Owin.Security.AuthenticationManagerExtensions.GetExternalLoginInfoAsync(manager, xsrfKey, expectedValue.ToString());
        }
    }
}