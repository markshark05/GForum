using System.Web.Mvc;

namespace GForum.Web.Controllers
{
    public class HomeController: Controller
    {
        // GET: /
        public ActionResult Index()
        {
            return View();
        }

        // GET: /Home/Index
        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}