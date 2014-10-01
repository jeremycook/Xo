using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Xo.Areas.Identity.Models;

namespace Xo.Infrastructure
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity Identity;
        private readonly ApplicationDbContext Db;
        private ApplicationUser _user;

        public CurrentUser(IIdentity identity, ApplicationDbContext db)
        {
            Identity = identity;
            Db = db;
        }

        public ApplicationUser User
        {
            get
            {
                if (_user == null)
                {
                    var userId = Xo.Areas.Identity.IdentityExtensions.GetUserId(Identity);
                    _user = Db.Users.SingleOrDefault(o => o.Id == userId);
                }

                return _user;
            }
        }
    }
}