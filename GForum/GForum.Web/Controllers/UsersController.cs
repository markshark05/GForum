using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using GForum.Web.IdentityConfig;
using GForum.Web.Models.Users;

namespace GForum.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationUserManager userManager;

        public UsersController(ApplicationUserManager userManager)
        {
            this.userManager = userManager;
        }

        // GET: /User/Username
        public ActionResult Index(string username)
        {
            var user = this.userManager.Users
                .Where(x => x.UserName == username)
                .ProjectTo<UserViewModel>()
                .FirstOrDefault();

            return View(user);
        }
    }
}