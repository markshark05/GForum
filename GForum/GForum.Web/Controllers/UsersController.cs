using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using GForum.Data;

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
                .FirstOrDefault(x => x.UserName == username);

            return View(user);
        }
    }
}