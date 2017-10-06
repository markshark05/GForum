using System.Web.Mvc;

namespace GForum.Web.Controllers
{
    public class HomeController: BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            this.ViewBag.Message = "Your application description page.";

            return View();
        }
    }
}