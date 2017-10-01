using System;
using System.Linq;
using System.Web.Mvc;
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
                .Select(x => new IndexViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    CategoriesCount = x.Categories.Count,
                    PostsCount = x.Posts.Count,
                    CreatedOn = x.CreatedOn ?? default(DateTime),
                })
                .FirstOrDefault();

            return View(user);
        }
    }
}