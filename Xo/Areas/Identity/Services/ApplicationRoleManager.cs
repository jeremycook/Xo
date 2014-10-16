﻿using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xo.Areas.Identity.Models;

namespace Xo.Areas.Identity.Services
{
    /// <remarks>
    /// Private because I don't want to accidentally use this instead of the Create method.
    /// </remarks>
    public class ApplicationRoleManager : RoleManager<Role, Guid>
    {
        private ApplicationRoleManager(RoleStore roleStore) : base(roleStore) { }

        public static ApplicationRoleManager Create(IdentityFactoryOptions<ApplicationRoleManager> options, IOwinContext context)
        {
            var roleManager = new ApplicationRoleManager(new RoleStore(context.Get<IdentityDbContext>()));
            return roleManager;
        }
    }
}