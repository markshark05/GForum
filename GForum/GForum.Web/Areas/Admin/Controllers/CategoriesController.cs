using System.Web.Mvc;
using GForum.Services.Contracts;

namespace GForum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController: Controller
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        // GET: /Admin/Categories
        public ActionResult Index()
        {
            var categories = this.categoryService.GetAll();

            return View(categories);
        }
    }
}