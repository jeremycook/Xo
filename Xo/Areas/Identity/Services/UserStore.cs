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
        private readonly IdentityDbContext Db;

        public UserStore(IdentityDbContext db)
        {
            this.Db = db;
        }

        public void Dispose()
        {
            // Do not dispose the IdentityDbContext so long as it is passed in.
        }

        //// IUserStore

        public async Task CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Db.Users.Add(user);
            await Db.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Db.Users.Remove(user);
            await Db.SaveChangesAsync();
        }

        public async Task<User> FindByIdAsync(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            return await Db.Users.SingleOrDefaultAsync(o => o.Id == userId);
        }

        public async Task<User> FindByNameAsync(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            return await Db.Users.SingleOrDefaultAsync(o => o.UserName == userName);
        }

        public async Task UpdateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Db.Entry(user).State = EntityState.Modified;
            await Db.SaveChangesAsync();
        }

        //// IUserPasswordStore

        public async Task<string> GetPasswordHashAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PasswordHash != null);
        }

        public async Task SetPasswordHashAsync(User user, string passwordHash)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (passwordHash == null)
            {
                throw new ArgumentNullException("passwordHash");
            }

            user.PasswordHash = passwordHash;
            await Db.SaveChangesAsync();
        }

        //// IUserEmailStore

        public async Task<User> FindByEmailAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            return await Db.Users.SingleOrDefaultAsync(o => o.Email == email);
        }

        public async Task<string> GetEmailAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailAsync(User user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Permit setting null email.
            user.Email = email;
            await Db.SaveChangesAsync();
        }

        public async Task SetEmailConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.EmailConfirmed = confirmed;
            await Db.SaveChangesAsync();
        }

        //// IUserPhoneNumberStore

        public async Task<string> GetPhoneNumberAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PhoneNumber);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberAsync(User user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Permit setting null phone number.
            user.PhoneNumber = phoneNumber;
            await Db.SaveChangesAsync();
        }

        public async Task SetPhoneNumberConfirmedAsync(User user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumberConfirmed = confirmed;
            await Db.SaveChangesAsync();
        }

        //// IUserTwoFactorStore

        public async Task<bool> GetTwoFactorEnabledAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.TwoFactorEnabled);
        }

        public async Task SetTwoFactorEnabledAsync(User user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.TwoFactorEnabled = enabled;
            await Db.SaveChangesAsync();
        }

        //// IUserLoginStore

        public async Task AddLoginAsync(User user, UserLoginInfo loginInfo)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (loginInfo == null)
            {
                throw new ArgumentNullException("loginInfo");
            }

            if (!await Db.Logins.AnyAsync(o =>
                o.UserId == user.Id &&
                o.LoginProvider == loginInfo.LoginProvider &&
                o.ProviderKey == loginInfo.ProviderKey))
            {
                Db.Logins.Add(new UserLogin(user.Id, loginInfo.LoginProvider, loginInfo.ProviderKey));
                await Db.SaveChangesAsync();
            }
        }

        public async Task<User> FindAsync(UserLoginInfo loginInfo)
        {
            if (loginInfo == null)
            {
                throw new ArgumentNullException("loginInfo");
            }

            return await Db.Users.SingleOrDefaultAsync(u =>
                Db.Logins
                    .Where(o => o.LoginProvider == loginInfo.LoginProvider && o.ProviderKey == loginInfo.ProviderKey)
                    .Select(o => o.UserId).Contains(u.Id));
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var logins = await Db.Logins
                    .Where(o => o.UserId == user.Id)
                    .ToListAsync();

            return logins
                .Select(o => new UserLoginInfo(o.LoginProvider, o.ProviderKey))
                .ToList();
        }

        public async Task RemoveLoginAsync(User user, UserLoginInfo loginInfo)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (loginInfo == null)
            {
                throw new ArgumentNullException("loginInfo");
            }

            var login = await Db.Logins
                .SingleOrDefaultAsync(o =>
                    o.UserId == user.Id &&
                    o.LoginProvider == loginInfo.LoginProvider &&
                    o.ProviderKey == loginInfo.ProviderKey);

            Db.Logins.Remove(login);
            await Db.SaveChangesAsync();
        }

        //// IUserLockoutStore

        public async Task<int> GetAccessFailedCountAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.LockoutEnabled);
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.LockoutEndDate);
        }

        public async Task<int> IncrementAccessFailedCountAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount += 1;
            await Db.SaveChangesAsync();
            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount = 0;
            await Db.SaveChangesAsync();
        }

        public async Task SetLockoutEnabledAsync(User user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEnabled = enabled;
            await Db.SaveChangesAsync();
        }

        public async Task SetLockoutEndDateAsync(User user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEndDate = lockoutEnd;
            await Db.SaveChangesAsync();
        }

        //// IUserSecurityStampStore

        public async Task<string> GetSecurityStampAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.SecurityStamp);
        }

        public async Task SetSecurityStampAsync(User user, string stamp)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (stamp == null)
            {
                throw new ArgumentNullException("stamp");
            }

            user.SecurityStamp = stamp;
            await Db.SaveChangesAsync();
        }

        //// IUserRoleStore

        public async Task AddToRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }

            var role = await Db.Roles.SingleOrDefaultAsync(o => o.Name.Equals(roleName, StringComparison.OrdinalIgnoreCase));
            user.Roles.Add(new UserRole(role.Id));
            await Db.SaveChangesAsync();
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
            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }

            return await Task.FromResult(user.Roles.Any(o => o.Role.Name == roleName));
        }

        public async Task RemoveFromRoleAsync(User user, string roleName)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }
            if (roleName == null)
            {
                throw new ArgumentNullException("roleName");
            }

            var userRole = user.Roles.SingleOrDefault(o => o.Role.Name == roleName);
            if (userRole != null)
            {
                user.Roles.Remove(userRole);
                await Db.SaveChangesAsync();
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

            Db.Claims.Add(new UserClaim(
                userId: user.Id,
                claimType: securityClaim.Type,
                claimValue: securityClaim.Value));
            await Db.SaveChangesAsync();
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
                await Db.SaveChangesAsync();
            }
        }
    }
}
