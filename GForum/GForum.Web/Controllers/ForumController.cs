using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GForum.Services;
using GForum.Web.Models.Forum;

namespace GForum.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly CategoryService categoryService;
        private readonly PostService postService;

        public ForumController(
                CategoryService categoryService,
                PostService postService
            )
        {
            this.categoryService = categoryService;
            this.postService = postService;
        }

        // GET: /forum
        public ActionResult Index()
        {
            var categories = this.categoryService.GetAll()
                .ProjectTo<CategoryViewModel>()
                .ToList();

            return View(categories);
        }

        // GET: /forum/category/id
        public ActionResult Category(Guid id)
        {
            var catgeory = Mapper.Map<CategoryWithPostsViewModel>(this.categoryService.GetById(id));

            return View(catgeory);
        }

        // GET: /forum/post/id
        public ActionResult Post(Guid id)
        {
            var post = Mapper.Map<PostViewModel>(this.postService.GetById(id));

            return View(post);
        }
    }
}
