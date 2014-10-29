using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xo.Areas.Users.Domain;

namespace Xo.Areas.Users.Services
{
    public class UserStore :
        IUserStore<UserAccount, Guid>,
        IUserPasswordStore<UserAccount, Guid>,
        IUserEmailStore<UserAccount, Guid>,
        IUserPhoneNumberStore<UserAccount, Guid>,
        IUserTwoFactorStore<UserAccount, Guid>,
        IUserLoginStore<UserAccount, Guid>,
        IUserLockoutStore<UserAccount, Guid>,
        IUserSecurityStampStore<UserAccount, Guid>,
        IUserRoleStore<UserAccount, Guid>,
        IUserClaimStore<UserAccount, Guid>
    {
        private readonly UsersDbContext Db;

        public UserStore(UsersDbContext db)
        {
            this.Db = db;
        }

        public void Dispose()
        {
            // Do not dispose the IdentityDbContext so long as it is passed in.
        }

        //// IUserStore

        public async Task CreateAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Db.UserAccounts.Add(user);
            await Db.SaveChangesAsync();
        }

        public async Task DeleteAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Db.UserAccounts.Remove(user);
            await Db.SaveChangesAsync();
        }

        public async Task<UserAccount> FindByIdAsync(Guid userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            return await Db.UserAccounts.SingleOrDefaultAsync(o => o.Id == userId);
        }

        public async Task<UserAccount> FindByNameAsync(string userName)
        {
            if (userName == null)
            {
                throw new ArgumentNullException("userName");
            }

            return await Db.UserAccounts.SingleOrDefaultAsync(o => o.UserName == userName);
        }

        public async Task UpdateAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            Db.Entry(user).State = EntityState.Modified;
            await Db.SaveChangesAsync();
        }

        //// IUserPasswordStore

        public async Task<string> GetPasswordHashAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PasswordHash);
        }

        public async Task<bool> HasPasswordAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PasswordHash != null);
        }

        public async Task SetPasswordHashAsync(UserAccount user, string passwordHash)
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

        public async Task<UserAccount> FindByEmailAsync(string email)
        {
            if (email == null)
            {
                throw new ArgumentNullException("email");
            }

            return await Db.UserAccounts.SingleOrDefaultAsync(o => o.Email == email);
        }

        public async Task<string> GetEmailAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.Email);
        }

        public async Task<bool> GetEmailConfirmedAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.EmailConfirmed);
        }

        public async Task SetEmailAsync(UserAccount user, string email)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Permit setting null email.
            user.Email = email;
            await Db.SaveChangesAsync();
        }

        public async Task SetEmailConfirmedAsync(UserAccount user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.EmailConfirmed = confirmed;
            await Db.SaveChangesAsync();
        }

        //// IUserPhoneNumberStore

        public async Task<string> GetPhoneNumberAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PhoneNumber);
        }

        public async Task<bool> GetPhoneNumberConfirmedAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.PhoneNumberConfirmed);
        }

        public async Task SetPhoneNumberAsync(UserAccount user, string phoneNumber)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Permit setting null phone number.
            user.PhoneNumber = phoneNumber;
            await Db.SaveChangesAsync();
        }

        public async Task SetPhoneNumberConfirmedAsync(UserAccount user, bool confirmed)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.PhoneNumberConfirmed = confirmed;
            await Db.SaveChangesAsync();
        }

        //// IUserTwoFactorStore

        public async Task<bool> GetTwoFactorEnabledAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.TwoFactorEnabled);
        }

        public async Task SetTwoFactorEnabledAsync(UserAccount user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.TwoFactorEnabled = enabled;
            await Db.SaveChangesAsync();
        }

        //// IUserLoginStore

        public async Task AddLoginAsync(UserAccount user, UserLoginInfo loginInfo)
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

        public async Task<UserAccount> FindAsync(UserLoginInfo loginInfo)
        {
            if (loginInfo == null)
            {
                throw new ArgumentNullException("loginInfo");
            }

            return await Db.UserAccounts.SingleOrDefaultAsync(u =>
                Db.Logins
                    .Where(o => o.LoginProvider == loginInfo.LoginProvider && o.ProviderKey == loginInfo.ProviderKey)
                    .Select(o => o.UserId).Contains(u.Id));
        }

        public async Task<IList<UserLoginInfo>> GetLoginsAsync(UserAccount user)
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

        public async Task RemoveLoginAsync(UserAccount user, UserLoginInfo loginInfo)
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

        public async Task<int> GetAccessFailedCountAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.AccessFailedCount);
        }

        public async Task<bool> GetLockoutEnabledAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.LockoutEnabled);
        }

        public async Task<DateTimeOffset> GetLockoutEndDateAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.LockoutEndDate);
        }

        public async Task<int> IncrementAccessFailedCountAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount += 1;
            await Db.SaveChangesAsync();
            return user.AccessFailedCount;
        }

        public async Task ResetAccessFailedCountAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.AccessFailedCount = 0;
            await Db.SaveChangesAsync();
        }

        public async Task SetLockoutEnabledAsync(UserAccount user, bool enabled)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEnabled = enabled;
            await Db.SaveChangesAsync();
        }

        public async Task SetLockoutEndDateAsync(UserAccount user, DateTimeOffset lockoutEnd)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            user.LockoutEndDate = lockoutEnd;
            await Db.SaveChangesAsync();
        }

        //// IUserSecurityStampStore

        public async Task<string> GetSecurityStampAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.SecurityStamp);
        }

        public async Task SetSecurityStampAsync(UserAccount user, string stamp)
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

        public async Task AddToRoleAsync(UserAccount user, string roleName)
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

        public async Task<IList<string>> GetRolesAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            return await Task.FromResult(user.Roles.Select(o => o.Role.Name).ToList());
        }

        public async Task<bool> IsInRoleAsync(UserAccount user, string roleName)
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

        public async Task RemoveFromRoleAsync(UserAccount user, string roleName)
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

        public async Task AddClaimAsync(UserAccount user, Claim securityClaim)
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

        public async Task<IList<Claim>> GetClaimsAsync(UserAccount user)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            var securityClaims = user.Claims.Select(o => o.ToSecurityClaim()).ToList();
            return await Task.FromResult(securityClaims);
        }

        public async Task RemoveClaimAsync(UserAccount user, Claim securityClaim)
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
                .SingleOrDefault(o => o.Type == securityClaim.Type && o.Value == securityClaim.Value);
            if (userClaim != null)
            {
                user.Claims.Remove(userClaim);
                await Db.SaveChangesAsync();
            }
        }
    }
}
