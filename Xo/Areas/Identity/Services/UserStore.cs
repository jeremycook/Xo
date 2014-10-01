using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xo.Areas.Identity.Models;

namespace Xo.Areas.Identity.Services
{
    public class UserStore :
        IUserStore<ApplicationUser, Guid>,
        IUserPasswordStore<ApplicationUser, Guid>,
        IUserEmailStore<ApplicationUser, Guid>,
        IUserPhoneNumberStore<ApplicationUser, Guid>,
        IUserTwoFactorStore<ApplicationUser, Guid>,
        IUserLoginStore<ApplicationUser, Guid>,
        IUserLockoutStore<ApplicationUser, Guid>,
        IUserSecurityStampStore<ApplicationUser, Guid>
    {
        private readonly ApplicationDbContext ApplicationDbContext;

        //// IUserStore

        public UserStore(ApplicationDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        public async Task CreateAsync(ApplicationUser user)
        {
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
            }

            ApplicationDbContext.Users.Add(user);
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(ApplicationUser user)
        {
            ApplicationDbContext.Users.Remove(user);
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> FindByIdAsync(Guid userId)
        {
            return await ApplicationDbContext.Users.SingleOrDefaultAsync(o => o.Id == userId);
        }

        public async Task<ApplicationUser> FindByNameAsync(string userName)
        {
            return await ApplicationDbContext.Users.SingleOrDefaultAsync(o => o.UserName == userName);
        }

        public async Task UpdateAsync(ApplicationUser user)
        {
            ApplicationDbContext.Entry(user).State = EntityState.Modified;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Do not dispose the ApplicationDbContext so long as it is passed in.
        }

        //// IUserPasswordStore

        public async Task<string> GetPasswordHashAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.PasswordHash != null);
        }

        public async Task SetPasswordHashAsync(ApplicationUser user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserEmailStore

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await ApplicationDbContext.Users.SingleOrDefaultAsync(o => o.Email == email);
        }

        public async Task<string> GetEmailAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailAsync(ApplicationUser user, string email)
        {
            user.Email = email;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task SetEmailConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserPhoneNumberStore

        public async Task<string> GetPhoneNumberAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.PhoneNumber);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberAsync(ApplicationUser user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task SetPhoneNumberConfirmedAsync(ApplicationUser user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserTwoFactorStore

        public async Task<bool> GetTwoFactorEnabledAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.TwoFactorEnabled);
        }

        public async Task SetTwoFactorEnabledAsync(ApplicationUser user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserLoginStore

        public async Task AddLoginAsync(ApplicationUser user, UserLoginInfo loginInfo)
        {
            ApplicationDbContext.Logins
                .Add(new IdentityUserLogin(user.Id, loginInfo.LoginProvider, loginInfo.ProviderKey));
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task<ApplicationUser> FindAsync(UserLoginInfo loginInfo)
        {
            return await ApplicationDbContext.Users.SingleOrDefaultAsync(u =>
                ApplicationDbContext.Logins
                    .Where(o => o.LoginProvider == loginInfo.LoginProvider && o.ProviderKey == loginInfo.ProviderKey)
                    .Select(o => o.UserId).Contains(u.Id));
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(ApplicationUser user)
        {
            var logins = await ApplicationDbContext.Logins
                    .Where(o => o.UserId == user.Id)
                    .ToListAsync();

            return logins
                .Select(o => new UserLoginInfo(o.LoginProvider, o.ProviderKey))
                .ToList();
        }

        public async Task RemoveLoginAsync(ApplicationUser user, UserLoginInfo loginInfo)
        {
            var login = await ApplicationDbContext.Logins
                .SingleOrDefaultAsync(o =>
                    o.UserId == user.Id &&
                    o.LoginProvider == loginInfo.LoginProvider &&
                    o.ProviderKey == loginInfo.ProviderKey);

            ApplicationDbContext.Logins.Remove(login);
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserLockoutStore

        public async Task<int> GetAccessFailedCountAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.LockoutEnabled);
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.LockoutEndDate);
        }

        public async Task<int> IncrementAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessFailedCount += 1;
            await ApplicationDbContext.SaveChangesAsync();
            // TODO: I'm suppossed to return an integer. Is this the right one?
            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(ApplicationUser user)
        {
            user.AccessFailedCount = 0;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task SetLockoutEnabledAsync(ApplicationUser user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task SetLockoutEndDateAsync(ApplicationUser user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDate = lockoutEnd;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserSecurityStampStore

        public async Task<string> GetSecurityStampAsync(ApplicationUser user)
        {
            return await Task.FromResult(user.SecurityStamp);
        }

        public async  Task SetSecurityStampAsync(ApplicationUser user, string stamp)
        {
            user.SecurityStamp = stamp;
            await ApplicationDbContext.SaveChangesAsync();
        }
    }
}
