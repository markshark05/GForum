using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using GForum.Data;
using GForum.Web.Models.Users;

namespace GForum.Web.Controllers
{
    public class UsersController : Controller
    {
        private readonly ApplicationData data;

        public UsersController(ApplicationData data)
        {
            this.data = data;
        }

        // GET: User/username
        public ActionResult Index(string username)
        {
            var user = this.data.Users.Query()
                .Where(x => x.UserName == username)
                .ProjectTo<UserViewModel>()
                .FirstOrDefault();

            return View(user);
        }
    }
}