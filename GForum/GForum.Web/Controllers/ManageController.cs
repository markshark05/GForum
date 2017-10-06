using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using GForum.Web.Models.Manage;
using GForum.Web.IdentityConfig;

namespace GForum.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private readonly ApplicationUserManager userManager;
        private readonly ApplicationSignInManager signInManager;

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            ChnageEmailSuccess,
            Error,
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }
        }

        // GET: /Manage/Index
        public ActionResult Index(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.ChnageEmailSuccess ? "Your email has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : string.Empty;

            return View();
        }

        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }
            var result = await this.userManager.ChangePasswordAsync(this.User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await this.userManager.FindByIdAsync(this.User.Identity.GetUserId());
                if (user != null)
                {
                    await this.signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        // GET: /Manage/ChangeEmail
        public ActionResult ChangeEmail()
        {
            var model = new ChangeEmailViewModel
            {
                CurrentEmail = this.userManager.GetEmail(this.User.Identity.GetUserId())
            };

            return View(model);
        }

        // POST: /Manage/ChangeEmail
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangeEmail(ChangeEmailViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var result = await this.userManager.SetEmailAsync(this.User.Identity.GetUserId(), model.NewEmail);
            if (result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.ChnageEmailSuccess });
            }
            AddErrors(result);
            return View(model);
        }
    }
}
