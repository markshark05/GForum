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
        private readonly PostService postService;

        public ForumController(CategoryService categoryService, PostService postService)
        {
            this.categoryService = categoryService;
            this.postService = postService;
        }

        // GET: /forum
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
            var category = this.categoryService
                .GetById(id);

            return View(category);
        }

        // GET: /forum/post/id
        public ActionResult Post(Guid id)
        {
            var post = this.postService
                .GetById(id);

            return View(post);
        }
    }
}
