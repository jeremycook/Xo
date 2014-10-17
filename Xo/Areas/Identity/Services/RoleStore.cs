using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Xo.Areas.Identity.Domain;

namespace Xo.Areas.Identity.Services
{
    public class RoleStore : 
        IRoleStore<Role, int>, 
        IQueryableRoleStore<Role, int>, 
        IDisposable
    {
        private readonly IdentityDbContext Db;

        public RoleStore(IdentityDbContext db)
        {
            Db = db;
        }

        //// IRoleStore
        
        public async Task CreateAsync(Role role)
        {
            Db.Roles.Add(role);
            await Db.SaveChangesAsync();
        }

        public async Task DeleteAsync(Role role)
        {
            Db.Roles.Remove(role);
            await Db.SaveChangesAsync();
        }

        public async Task<Role> FindByIdAsync(int roleId)
        {
            var id = (RoleId)roleId;
            return await Db.Roles.SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<Role> FindByNameAsync(string roleName)
        {
            return await Db.Roles.SingleOrDefaultAsync(o => o.Name == roleName);
        }

        public async Task UpdateAsync(Role role)
        {
            Db.Entry(role).State = EntityState.Modified;
            await Db.SaveChangesAsync();
        }

        //// IQueryableRoleStore

        public IQueryable<Role> Roles
        {
            get { throw new NotImplementedException(); }
        }

        //// IDisposable

        public void Dispose()
        {
            // Do not dispose the Db so long as it is passed in.
        }
    }
}