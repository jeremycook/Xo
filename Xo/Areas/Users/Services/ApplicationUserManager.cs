using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Xo.Areas.Users.Domain;

namespace Xo.Areas.Users.Services
{
    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Users and is used by the application.
    public class ApplicationUserManager : UserManager<UserAccount, Guid>
    {
        /// <remarks>
        /// Private because I don't want to accidentally use this instead of the Create method.
        /// </remarks>
        private ApplicationUserManager(IUserStore<UserAccount, Guid> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
        {
            var manager = new ApplicationUserManager(new UserStore(context.Get<UsersDbContext>()));

            // Configure validation logic for usernames
            manager.UserValidator = new UserValidator<UserAccount, Guid>(manager)
            {
                AllowOnlyAlphanumericUserNames = false,
                RequireUniqueEmail = true
            };

            // Configure validation logic for passwords
            manager.PasswordValidator = new PasswordValidator
            {
                RequiredLength = 6,
                RequireNonLetterOrDigit = true,
                RequireDigit = true,
                RequireLowercase = true,
                RequireUppercase = true,
            };

            // Configure user lockout defaults
            manager.UserLockoutEnabledByDefault = true;
            manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            // You can write your own provider and plug it in here.
            manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<UserAccount, Guid>
            {
                MessageFormat = "Your security code is {0}"
            });
            manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<UserAccount, Guid>
            {
                Subject = "Security Code",
                BodyFormat = "Your security code is {0}"
            });
            manager.EmailService = new EmailService();
            manager.SmsService = new SmsService();

            // Encryption layer
            var dataProtectionProvider = options.DataProtectionProvider;
            if (dataProtectionProvider != null)
            {
                // Using the purpose "ASP.NET Users" for now because I'm not sure
                // if that same purpose is used elsewhere by other code.
                manager.UserTokenProvider = new DataProtectorTokenProvider<UserAccount, Guid>(
                    dataProtectionProvider.Create("ASP.NET Users"));
            }

            return manager;
        }
    }
}