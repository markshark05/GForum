using System.Web.Mvc;
using AutoMapper;
using GForum.Services.Contracts;
using GForum.Web.Areas.Admin.Models;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IMapper mapper;

        public CategoriesController(
            ICategoryService categoryService,
            IMapper mapper)
        {
            this.categoryService = categoryService;
            this.mapper = mapper;
        }

        // GET: /Admin/Categories
        public ActionResult Index()
        {
            var model = new AdminCategoriesViewModel
            {
                CategoriesQueriable = this.categoryService.GetAll(),
                CategoryAdd = new CategoryAddViewModel()
            };

            return View(model);
        }

        // GET: /Admin/Categories/Add
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Add(CategoryAddViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var userId = this.User.Identity.GetUserId();
            var post = this.categoryService
                .Create(userId, model.Title);

            return RedirectToAction("Index");
        }
    }
}