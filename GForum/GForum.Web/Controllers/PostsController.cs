using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper;
using GForum.Services.Contracts;
using GForum.Web.Contracts.Identity;
using GForum.Web.Models.Comments;
using GForum.Web.Models.Posts;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class PostsController : Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;
        private readonly IVoteService voteService;
        private readonly ICommentService commentService;
        private readonly IApplicationUserManager userManager;
        private readonly IMapper mapper;

        public PostsController(
            ICategoryService categoryService,
            IPostService postService,
            IVoteService voteService,
            ICommentService commentService,
            IApplicationUserManager userManager,
            IMapper mapper)
        {
            this.categoryService = categoryService;
            this.postService = postService;
            this.voteService = voteService;
            this.commentService = commentService;
            this.userManager = userManager;
            this.mapper = mapper;
        }

        // GET: /Posts/Post/{id}
        public ActionResult Post(Guid id)
        {
            var post = this.postService.GetById(id)
                .Select(this.mapper.Map<PostViewModel>)
                .FirstOrDefault();

            if (post == null)
            {
                return HttpNotFound();
            }

            if (this.Request.IsAuthenticated)
            {
                post.UserVoteType = this.voteService
                    .GetUserVoteTypeForPost(post.Id, this.User.Identity.GetUserId());
            }

            return View(post);
        }

        // GET: /Posts/Submit {categoryId}
        [Authorize]
        public ActionResult Submit(Guid catgeoryId)
        {
            var model = this.categoryService.GetById(catgeoryId)
                .Select(this.mapper.Map<PostSubmitViewModel>)
                .FirstOrDefault();

            if (model == null)
            {
                return HttpNotFound();
            }

            return View(model);
        }

        // POST: /Posts/Submit {Model}
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

            var category = this.categoryService.GetById(model.CategoryId)
                .FirstOrDefault();

            if (category == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = this.User.Identity.GetUserId();
            var post = this.postService
                .Submit(model.CategoryId, userId, model.Title, model.Content);

            return RedirectToAction("Post", new { id = post.Id });
        }

        // POST: /Posts/Comment {Model}
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Comment(CommentAddViewModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            var post = this.postService.GetById(model.PostId)
                .FirstOrDefault();

            if (post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = this.User.Identity.GetUserId();
            this.commentService.AddCommentToPost(model.PostId, userId, model.Content);

            return RedirectToAction("Post", new { id = model.PostId });
        }
    }
}