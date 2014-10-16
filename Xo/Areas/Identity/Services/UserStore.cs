using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xo.Areas.Identity.Models;

namespace Xo.Areas.Identity.Services
{
    public class UserStore :
        IUserStore<User, Guid>,
        IUserPasswordStore<User, Guid>,
        IUserEmailStore<User, Guid>,
        IUserPhoneNumberStore<User, Guid>,
        IUserTwoFactorStore<User, Guid>,
        IUserLoginStore<User, Guid>,
        IUserLockoutStore<User, Guid>,
        IUserSecurityStampStore<User, Guid>,
        IUserRoleStore<User, Guid>,
        IUserClaimStore<User, Guid>
    {
        private readonly IdentityDbContext ApplicationDbContext;

        //// IUserStore

        public UserStore(IdentityDbContext applicationDbContext)
        {
            ApplicationDbContext = applicationDbContext;
        }

        public async Task CreateAsync(User user)
        {
            if (user.Id == Guid.Empty)
            {
                user.Id = Guid.NewGuid();
            }

            ApplicationDbContext.Users.Add(user);
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            ApplicationDbContext.Users.Remove(user);
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task<User> FindByIdAsync(Guid userId)
        {
            return await ApplicationDbContext.Users.SingleOrDefaultAsync(o => o.Id == userId);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            return await ApplicationDbContext.Users.SingleOrDefaultAsync(o => o.UserName == userName);
        }

        public async Task UpdateAsync(User user)
        {
            ApplicationDbContext.Entry(user).State = EntityState.Modified;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public void Dispose()
        {
            // Do not dispose the ApplicationDbContext so long as it is passed in.
        }

        //// IUserPasswordStore

        public async Task<string> GetPasswordHashAsync(User user)
        {
            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(User user)
        {
            return await Task.FromResult(user.PasswordHash != null);
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserEmailStore

        public async Task<User> FindByEmailAsync(string email)
        {
            return await ApplicationDbContext.Users.SingleOrDefaultAsync(o => o.Email == email);
        }

        public async Task<string> GetEmailAsync(User user)
        {
            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user)
        {
            return await Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailAsync(User user, string email)
        {
            user.Email = email;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            user.EmailConfirmed = confirmed;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserPhoneNumberStore

        public async Task<string> GetPhoneNumberAsync(User user)
        {
            return await Task.FromResult(user.PhoneNumber);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            user.PhoneNumber = phoneNumber;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            user.PhoneNumberConfirmed = confirmed;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserTwoFactorStore

        public async Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            return await Task.FromResult(user.TwoFactorEnabled);
        }

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            user.TwoFactorEnabled = enabled;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserLoginStore

        public async Task AddLoginAsync(User user, UserLoginInfo loginInfo)
        {
            ApplicationDbContext.Logins
                .Add(new UserLogin(user.Id, loginInfo.LoginProvider, loginInfo.ProviderKey));
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task<User> FindAsync(UserLoginInfo loginInfo)
        {
            return await ApplicationDbContext.Users.SingleOrDefaultAsync(u =>
                ApplicationDbContext.Logins
                    .Where(o => o.LoginProvider == loginInfo.LoginProvider && o.ProviderKey == loginInfo.ProviderKey)
                    .Select(o => o.UserId).Contains(u.Id));
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            var logins = await ApplicationDbContext.Logins
                    .Where(o => o.UserId == user.Id)
                    .ToListAsync();

            return logins
                .Select(o => new UserLoginInfo(o.LoginProvider, o.ProviderKey))
                .ToList();
        }

        public async Task RemoveLoginAsync(User user, UserLoginInfo loginInfo)
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

        public async Task<int> GetAccessFailedCountAsync(User user)
        {
            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user)
        {
            return await Task.FromResult(user.LockoutEnabled);
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            return await Task.FromResult(user.LockoutEndDate);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount += 1;
            await ApplicationDbContext.SaveChangesAsync();
            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(User user)
        {
            user.AccessFailedCount = 0;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            user.LockoutEnabled = enabled;
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            user.LockoutEndDate = lockoutEnd;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserSecurityStampStore

        public async Task<string> GetSecurityStampAsync(User user)
        {
            return await Task.FromResult(user.SecurityStamp);
        }

        public async Task SetSecurityStampAsync(User user, string stamp)
        {
            user.SecurityStamp = stamp;
            await ApplicationDbContext.SaveChangesAsync();
        }

        //// IUserRoleStore

        public async Task AddToRoleAsync(User user, string roleName)
        {
            var role = await ApplicationDbContext.Roles.SingleOrDefaultAsync(o => o.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            user.Roles.Add(new UserRole(role));
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task<IList<string>> GetRolesAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.Roles.Select(o => o.Role.Name).ToList());
        }

        public async Task<bool> IsInRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.Roles.Any(o => o.Role.Name == roleName));
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var userRole = user.Roles.SingleOrDefault(o => o.Role.Name == roleName);
            if (userRole != null)
            {
                user.Roles.Remove(userRole);
                await ApplicationDbContext.SaveChangesAsync();
            }
        }

        //// IUserClaimStore

        public async Task AddClaimAsync(User user, Claim securityClaim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (securityClaim == null)
            {
                throw new ArgumentNullException("securityClaim");
            }

            ApplicationDbContext.Claims.Add(new UserClaim(
                userId: user.Id,
                claimType: securityClaim.Type,
                claimValue: securityClaim.Value));
            await ApplicationDbContext.SaveChangesAsync();
        }

        public async Task<IList<Claim>> GetClaimsAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var securityClaims = user.Claims.Select(o => o.ToSecurityClaim()).ToList();
            return await Task.FromResult(securityClaims);
        }

        public async Task RemoveClaimAsync(User user, Claim securityClaim)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (securityClaim == null)
            {
                throw new ArgumentNullException("securityClaim");
            }

            var userClaim = user.Claims
                .SingleOrDefault(o => o.ClaimType == securityClaim.Type && o.ClaimValue == securityClaim.Value);
            if (userClaim != null)
            {
                user.Claims.Remove(userClaim);
                await ApplicationDbContext.SaveChangesAsync();
            }
        }
    }
}
