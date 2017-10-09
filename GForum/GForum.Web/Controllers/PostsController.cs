using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using GForum.Data.Models;
using GForum.Services.Contracts;
using GForum.Web.Identity;
using GForum.Web.Models.Forum;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class PostsController: Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;
        private readonly ApplicationUserManager userManager;

        public PostsController(
            ICategoryService categoryService,
            IPostService postService,
            ApplicationUserManager userManager)
        {
            this.categoryService = categoryService;
            this.postService = postService;
            this.userManager = userManager;
        }

        // GET: /Posts/Post/{id}
        public ActionResult Post(Guid id)
        {
            var post = this.postService.GetById(id)
                .ProjectTo<PostViewModel>()
                .FirstOrDefault();

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

        // GET: /Posts/Submit/{categoryId}
        [Authorize]
        public ActionResult Submit(Guid catgeoryId)
        {
            var model = this.categoryService.GetById(catgeoryId)
                .ProjectTo<PostSubmitViewModel>()
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

            var post = new Post
            {
                Title = model.Title,
                Content = model.Content,
                CategoryId = model.CategoryId,
                AuthorId = this.User.Identity.GetUserId(),
            };
            this.postService.Submit(post);

            return RedirectToAction("Post", new { id = post.Id });
        }
    }
}