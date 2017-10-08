using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using GForum.Common.Enums;
using GForum.Data.Models;
using GForum.Services;
using GForum.Web.Helpers;
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
            var categories = this.categoryService.GetQueryable()
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

        // GET: /Forum/Post/Id
        public ActionResult Post(Guid id)
        {
            var post = Mapper.Map<Post, PostViewModel>(this.postService.GetById(id));
            if (post == null)
            {
                return HttpNotFound();
            }

            if (this.Request.IsAuthenticated)
            {
                post.UserVoteType = this.postService
                    .GetUserVoteTypeForPost(post.Id, this.User.Identity.GetUserId());
            }

            return View(post);
        }

        // GET: /Forum/Category/Id/Submit
        [Authorize]
        public ActionResult Submit(Guid catgeoryId)
        {
            var model = Mapper.Map<Category, PostSubmitViewModel>(this.categoryService.GetById(catgeoryId));
            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        // POST: /Forum/Category/Id/Submit
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
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

        // POST: /Forum/Vote {voteType, postId}
        [HttpPost]
        [AjaxAuthorize]
        public ActionResult Vote(Guid postId, VoteType voteType)
        {
            var enumIsValid = Enum.IsDefined(typeof(VoteType), voteType) && voteType != VoteType.None;
            var post = this.postService.GetById(postId);
            if (!enumIsValid || post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            this.postService.ToggleVote(postId, new Vote
            {
                UserId = this.User.Identity.GetUserId(),
                PostId = postId,
                VoteType = voteType,
            });

            return Json(new
            {
                success = true,
                likes = post.VoteCount.ToMetric()
            });
        }

        // POST: /Forum/Edit {postId, newContent}
        [HttpPost]
        [AjaxAuthorize]
        [ValidateInput(false)]
        public ActionResult Edit(Guid postId, string content)
        {
            var post = this.postService.GetById(postId);
            if (post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (post.AuthorId != this.User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            if (string.IsNullOrWhiteSpace(content))
            {
                return Json(new
                {
                    success = false,
                    error = "Content cannot be empty."
                });
            }

            this.postService.Edit(postId, content);

            return Json(new { success = true });
        }

        // POST: /Forum/Edit {postId, newContent}
        [HttpPost]
        [AjaxAuthorize]
        [ValidateInput(false)]
        public ActionResult Delete(Guid postId)
        {
            var post = this.postService.GetById(postId);
            if (post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            }

            if (post.AuthorId != this.User.Identity.GetUserId())
            {
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);
            }

            this.postService.Delete(postId);

            return Json(new { success = true });
        }
    }
}