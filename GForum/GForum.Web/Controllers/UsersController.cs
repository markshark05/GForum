using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GForum.Web.Identity;
using GForum.Web.Models.Users;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationUserManager userManager;
        private readonly IMapper mapper;

        public UsersController(
            ApplicationUserManager userManager,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.mapper = mapper;
        }

        // GET: /Users/Username
        [ActionName("Profile")]
        public ActionResult UserProfile(string username)
        {
            var user = this.userManager.Users
                .Where(x => x.UserName == username)
                .Select(this.mapper.Map<UserViewModel>)
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