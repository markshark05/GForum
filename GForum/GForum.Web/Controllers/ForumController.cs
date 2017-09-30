using System;
using System.Linq;
using System.Web.Mvc;
using GForum.Services;
using GForum.Web.Models.Forum;

namespace GForum.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly CategoryService categoryService;

        public ForumController(CategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        public ActionResult Index()
        {
            var indexViewModel = new ForumIndexViewModel
            {
                Categories = this.categoryService
                    .GetAll()
                    .Select(c => new ForumIndexViewModel.Category
                    {
                        Id = c.Id,
                        Author = new ForumIndexViewModel.Author
                        {
                            Id = c.Author.Id,
                            UserName = c.Author.UserName,
                        },
                        Title = c.Title,
                        CreatedOn = c.CreatedOn ?? default(DateTime),
                        PostsCount = c.Posts.Count,
                    })
            };

            return View(indexViewModel);
        }

        // GET: /forum/category/id
        public ActionResult Category(Guid id)
        {
            var category = this.categoryService.GetById(id);

            return View(category);
        }
    }
}
