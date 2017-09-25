using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using GForum.Web.Models.Manage;
using GForum.Web.IdentityConfig;
using System.Web.Routing;

namespace GForum.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationUserManager userManager;
        private ApplicationSignInManager signInManager;
        private IAuthenticationManager authenticationManager;

        protected override void Initialize(RequestContext requestContext)
        {
            this.userManager = requestContext.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            this.signInManager = requestContext.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            this.authenticationManager = requestContext.HttpContext.GetOwinContext().Authentication;

            base.Initialize(requestContext);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.userManager?.Dispose();
                this.signInManager?.Dispose();
            }

            base.Dispose(disposing);
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            Error
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError(string.Empty, error);
            }
        }

        private bool HasPassword()
        {
            var user = this.userManager.FindById(this.User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        // GET: /Manage/Index
        public ActionResult Index(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : string.Empty;

            var userId = this.User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
            };
            return View(model);
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
    }
}
