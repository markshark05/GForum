using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using GForum.Web.Models.Users;

namespace GForum.Web.Controllers
{
    public class UsersController : BaseController
    {
        // GET: User/Username
        public ActionResult Index(string username)
        {
            var user = this.UserManager.Users
                .Where(x => x.UserName == username)
                .ProjectTo<UserViewModel>()
                .FirstOrDefault();

            return View(user);
        }
    }
}