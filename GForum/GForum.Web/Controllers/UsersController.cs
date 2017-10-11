using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GForum.Web.Contracts.Identity;
using GForum.Web.Models.Users;

namespace GForum.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly IApplicationUserManager userManager;
        private readonly IMapper mapper;

        public UsersController(
            IApplicationUserManager userManager,
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
    }
}