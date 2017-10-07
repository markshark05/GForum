using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GForum.Common.Enums;
using GForum.Data.Models;
using GForum.Services;
using GForum.Web.IdentityConfig;
using GForum.Web.Models.Forum;
using Humanizer;
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
            var post = Mapper.Map<Post, PostViewModel>(this.postService.GetById(id));
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
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
        public ActionResult Vote(Guid postId, VoteType voteType)
        {
            if (!this.Request.IsAjaxRequest())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var enumIsDefined = Enum.IsDefined(typeof(VoteType), voteType);
            var post = this.postService.GetById(postId);
            if (!enumIsDefined || post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.postService.ToggleVote(post, new Vote
            {
                UserId = this.User.Identity.GetUserId(),
                PostId = post.Id,
                VoteType = voteType,
            });

            return Content(post.VoteCount.ToMetric());
        }
    }
}