using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Xo.Areas.Identity.Domain;
using Xo.Areas.Identity.Models;
using Xo.Areas.Identity.Services;
using Xo.Infrastructure;
using Xo.Infrastructure.Alerts;

namespace Xo.Areas.Identity.Controllers
{
    [Authorize]
    [RouteArea("Identity", AreaPrefix = "")]
    [Route("Manage/{Action=Index}")]
    public class ManageController : Controller
    {
        private readonly ApplicationUserManager UserManager;
        private readonly IAuthenticationManager AuthenticationManager;
        private readonly ICurrentUser CurrentUser;

        public ManageController(ApplicationUserManager userManager, IAuthenticationManager authenticationManager, ICurrentUser currentUser)
        {
            UserManager = userManager;
            AuthenticationManager = authenticationManager;
            CurrentUser = currentUser;
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync((int)CurrentUser.User.Id),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync((int)CurrentUser.User.Id),
                Logins = await UserManager.GetLoginsAsync((int)CurrentUser.User.Id),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync((int)CurrentUser.User.Id)
            };
            var result = View(model);
            return message == ManageMessageId.ChangePasswordSuccess ? result.WithSuccess("Your password has been changed.")
                : message == ManageMessageId.SetPasswordSuccess ? result.WithSuccess("Your password has been set.")
                : message == ManageMessageId.SetTwoFactorSuccess ? result.WithSuccess("Your two-factor authentication provider has been set.")
                : message == ManageMessageId.Error ? result.WithError("An error has occurred.")
                : message == ManageMessageId.AddPhoneSuccess ? result.WithSuccess("Your phone number was added.")
                : message == ManageMessageId.RemovePhoneSuccess ? result.WithSuccess("Your phone number was removed.")
                : result;
        }

        //
        // GET: /Manage/RemoveLogin
        public ActionResult RemoveLogin()
        {
            var linkedAccounts = UserManager.GetLogins((int)CurrentUser.User.Id);
            ViewBag.ShowRemoveButton = HasPassword() || linkedAccounts.Count > 1;
            return View(linkedAccounts);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync((int)CurrentUser.User.Id, new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = CurrentUser.User;
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync((int)CurrentUser.User.Id, model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number })
                .WithInfo("A security code has been sent to the phone number you provided.");
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync((int)CurrentUser.User.Id, true);
            var user = CurrentUser.User;
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage")
                .WithSuccess("Two factor authentication is enabled for your account.");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync((int)CurrentUser.User.Id, false);
            var user = CurrentUser.User;
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", "Manage")
                .WithSuccess("Two factor authentication is disabled for your account.");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync((int)CurrentUser.User.Id, phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync((int)CurrentUser.User.Id, model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = CurrentUser.User;
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // GET: /Manage/RemovePhoneNumber
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync((int)CurrentUser.User.Id, null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = CurrentUser.User;
            if (user != null)
            {
                await SignInAsync(user, isPersistent: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync((int)CurrentUser.User.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = CurrentUser.User;
                if (user != null)
                {
                    await SignInAsync(user, isPersistent: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync((int)CurrentUser.User.Id, model.NewPassword);
                if (result.Succeeded)
                {
                    var user = CurrentUser.User;
                    if (user != null)
                    {
                        await SignInAsync(user, isPersistent: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            var user = CurrentUser.User;
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync((int)CurrentUser.User.Id);
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;

            var result = View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
            return message == ManageMessageId.RemoveLoginSuccess ? result.WithSuccess("The external login was removed.")
                : message == ManageMessageId.Error ? result.WithError("An error has occurred.")
                : result;
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), CurrentUser.User.Id);
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, (int)CurrentUser.User.Id);
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync((int)CurrentUser.User.Id, loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private async Task SignInAsync(User user, bool isPersistent)
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
            AuthenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(UserManager));
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById((int)CurrentUser.User.Id);
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById((int)CurrentUser.User.Id);
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}