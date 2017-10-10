using System;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GForum.Services.Contracts;
using GForum.Web.Identity;
using GForum.Web.Models.Forum;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;
        private readonly IVoteService voteService;
        private readonly ApplicationUserManager userManager;
        private readonly IMapper mapper;

        public CategoriesController(
            ICategoryService categoryService,
            IPostService postService,
            IVoteService voteService,
            ApplicationUserManager userManager,
            IMapper mapper)
        {
            this.categoryService = categoryService;
            this.postService = postService;
            this.voteService = voteService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        // GET: /Categories
        public ActionResult Index()
        {
            var categories = this.categoryService.GetAll()
                .Select(this.mapper.Map<CategoryViewModel>)
                .ToList();

            return View(categories);
        }

        // GET: /Categories/Category/{id}
        public ActionResult Category(Guid id)
        {
            var category = this.categoryService.GetById(id)
                .Select(this.mapper.Map<CategoryWithPostsViewModel>)
                .FirstOrDefault();

            if (category == null)
            {
                return HttpNotFound();
            }

            if (this.Request.IsAuthenticated)
            {
                foreach (var post in category.Posts)
                {
                    post.UserVoteType = this.voteService
                        .GetUserVoteTypeForPost(post.Id, this.User.Identity.GetUserId());
                }
            }

            return View(category);
        }
    }
}