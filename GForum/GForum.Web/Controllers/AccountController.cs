﻿using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using GForum.Web.Models.Account;
using GForum.Data.Models;
using Microsoft.Owin.Security;
using GForum.Web.Contracts.Identity;
using System.Linq;

namespace GForum.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IApplicationUserManager userManager;
        private readonly IApplicationSignInManager signInManager;
        private readonly IAuthenticationManager authenticationManager;

        public AccountController(
            IApplicationUserManager userManager,
            IApplicationSignInManager signInManager,
            IAuthenticationManager authenticationManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.authenticationManager = authenticationManager;
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.signInManager
                .PasswordSignInAsync(model.Username, model.Password, model.RememberMe, shouldLockout: true);
            switch (result)
            {
                case SignInStatus.Success:
                    return RedirectToLocal(returnUrl);
                default:
                    this.ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
            }
        }

        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await this.userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    await this.signInManager
                        .SignInAsync(user, isPersistent: true, rememberBrowser: false);

                    return RedirectToAction("Index", "Home");
                }

                AddErrors(result);
            }

            return View(model);
        }

        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.authenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public ActionResult UserAvatar()
        {
            var userId = this.User.Identity.GetUserId();
            var user = this.userManager.Users
                    .Where(x => x.Id == userId)
                    .FirstOrDefault();

            if (user == null)
            {
                return new EmptyResult();
            }

            return PartialView("_UserAvatarPartial", user.Email);
        }
    }
}
