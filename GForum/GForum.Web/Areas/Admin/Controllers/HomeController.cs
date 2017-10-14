using System.Web.Mvc;

namespace GForum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        // GET: /Admin/
        public ActionResult Index()
        {
            return View();
        }
    }
}