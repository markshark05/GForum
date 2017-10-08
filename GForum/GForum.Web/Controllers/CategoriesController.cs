using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using GForum.Services.Contracts;
using GForum.Web.IdentityConfig;
using GForum.Web.Models.Forum;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;
        private readonly ApplicationUserManager userManager;

        public CategoriesController(
            ICategoryService categoryService,
            IPostService postService,
            ApplicationUserManager userManager)
        {
            this.categoryService = categoryService;
            this.postService = postService;
            this.userManager = userManager;
        }

        // GET: /Categories
        public ActionResult Index()
        {
            var categories = this.categoryService.GetAll()
                .ProjectTo<CategoryViewModel>()
                .ToList();

            return View(categories);
        }

        // GET: /Categories/Category/{id}
        public ActionResult Category(Guid id)
        {
            var category = this.categoryService.GetById(id)
                .ProjectTo<CategoryWithPostsViewModel>()
                .FirstOrDefault();

            if (category == null)
            {
                return HttpNotFound();
            }

            if (this.Request.IsAuthenticated)
            {
                foreach (var post in category.Posts)
                {
                    post.UserVoteType = this.postService
                        .GetUserVoteTypeForPost(post.Id, this.User.Identity.GetUserId());
                }
            }

            return View(category);
        }
    }
}