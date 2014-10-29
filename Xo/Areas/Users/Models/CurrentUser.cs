using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Web;
using Xo.Areas.Users.Domain;

namespace Xo.Areas.Users.Models
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity Users;
        private readonly UsersDbContext Db;
        private UserAccount _user;

        public CurrentUser(IIdentity identity, UsersDbContext db)
        {
            Users = identity;
            Db = db;
        }

        public UserAccount User
        {
            get
            {
                if (_user == null)
                {
                    var userId = Xo.Areas.Users.IdentityExtensions.GetUserId(Users);
                    _user = Db.UserAccounts.SingleOrDefault(o => o.Id == userId);
                }

                return _user;
            }
        }
    }
}