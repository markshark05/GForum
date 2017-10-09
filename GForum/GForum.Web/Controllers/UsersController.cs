using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using GForum.Web.Identity;
using GForum.Web.Models.Users;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationUserManager userManager;

        public UsersController(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }

        // GET: /Users/Username
        [ActionName("Profile")]
        public ActionResult UserProfile(string username)
        {
            var user = this.userManager.Users
                .Where(x => x.UserName == username)
                .ProjectTo<UserViewModel>()
                .FirstOrDefault();

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [ChildActionOnly]
        public string GetEmail()
        {
            return this.userManager.GetEmail(this.User.Identity.GetUserId());
        }
    }
}