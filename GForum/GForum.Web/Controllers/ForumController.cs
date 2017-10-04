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
            return View(this.categoryService
                .GetAll()
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Author = new AuthorViewModel
                    {
                        Id = c.Author.Id,
                        UserName = c.Author.UserName,
                    },
                    Title = c.Title,
                    CreatedOn = c.CreatedOn ?? default(DateTime),
                    PostsCount = c.Posts.Count,
                })
                .ToList()
            );
        }

        // GET: /forum/category/id
        public ActionResult Category(Guid id)
        {
            var c = this.categoryService
                .GetById(id);

            return View(new CategoryWithPostsViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Posts = c.Posts.Select(p => new PostViewModel
                {
                    Id = p.Id,
                    Title = p.Title,
                    Author = new AuthorViewModel
                    {
                        Id = p.Author.Id,
                        UserName = p.Author.UserName,
                    },
                    CreatedOn = p.CreatedOn ?? default(DateTime),
                })
            });
        }

        // GET: /forum/post/id
        public ActionResult Post(Guid id)
        {
            var p = this.postService
                .GetById(id);

            return View(new PostViewModel
            {
                Id = p.Id,
                Title = p.Title,
                Content = p.Content,
                Author = new AuthorViewModel
                {
                    Id = p.Author.Id,
                    UserName = p.Author.UserName,
                },
                CreatedOn = p.CreatedOn ?? default(DateTime),
            });
        }
    }
}
