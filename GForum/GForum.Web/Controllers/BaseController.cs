using System.Web;
using System.Web.Mvc;
using GForum.Web.IdentityConfig;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace GForum.Web.Controllers
{
    public class BaseController : Controller
    {
        protected ApplicationUserManager UserManager => this.HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();

        protected ApplicationSignInManager SignInManager => this.HttpContext.GetOwinContext().Get<ApplicationSignInManager>();

        protected IAuthenticationManager AuthenticationManager => this.HttpContext.GetOwinContext().Authentication;

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.UserManager?.Dispose();
                this.SignInManager?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}