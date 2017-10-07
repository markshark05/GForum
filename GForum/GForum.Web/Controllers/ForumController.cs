using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GForum.Data.Models;
using GForum.Services;
using GForum.Web.IdentityConfig;
using GForum.Web.Models.Forum;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class ForumController : Controller
    {
        private readonly CategoryService categoryService;
        private readonly PostService postService;
        private readonly ApplicationUserManager userManager;

        public ForumController(
            CategoryService categoryService,
            PostService postService,
            ApplicationUserManager userManager)
        {
            this.categoryService = categoryService;
            this.postService = postService;
            this.userManager = userManager;
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
        [Authorize]
        public ActionResult Submit(Guid catgeoryId)
        {
            var model = Mapper.Map<PostSubmitViewModel>(this.categoryService.GetById(catgeoryId));
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        // POST: /Forum/Category/Id/Submit
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(PostSubmitViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var category = this.categoryService.GetById(model.CategoryId);
            if (category == null)
            {
                return HttpNotFound();
            }

            this.postService.Submit(new Post
            {
                Title = model.Title,
                Content = model.Content,
                CategoryId = model.CategoryId,
                AuthorId = this.User.Identity.GetUserId(),
            });
            return RedirectToAction("Category", "Forum", new { id = model.CategoryId });
        }

        // POST: /Forum/Category/Id/Submit
        [HttpPost]
        [Authorize]
        public ActionResult Vote()
        {
            if (!this.Request.IsAjaxRequest())
            {
                return HttpNotFound();
            }

            return new EmptyResult();
        }
    }
}