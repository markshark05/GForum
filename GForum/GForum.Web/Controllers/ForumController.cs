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

        // GET: /Forum
        public ActionResult Index()
        {
            var categories = this.categoryService.GetAll()
                .ProjectTo<CategoryViewModel>()
                .ToList();

            return View(categories);
        }

        // GET: /Forum/Category/Id
        public ActionResult Category(Guid id)
        {
            var category = Mapper.Map<CategoryWithPostsViewModel>(this.categoryService.GetById(id));
            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // GET: /Forum/Post/Id
        public ActionResult Post(Guid id)
        {
            var post = Mapper.Map<PostViewModel>(this.postService.GetById(id));
            if (post == null)
            {
                return HttpNotFound();
            }

            return View(post);
        }

        // GET: /Forum/Category/Id/Submit
        public ActionResult Submit(Guid catgeoryId)
        {
            var category = Mapper.Map<CategoryViewModel>(this.categoryService.GetById(catgeoryId));
            if (category == null)
            {
                return HttpNotFound();
            }

            var model = new PostSubmitViewModel
            {
                Category = category,
            };
            return View(model);
        }
    }
}
