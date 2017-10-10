using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using GForum.Common.Enums;
using GForum.Services.Contracts;
using GForum.Web.Identity;
using Humanizer;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class PostsAjaxController: Controller
    {
        private readonly ICategoryService categoryService;
        private readonly IPostService postService;
        private readonly ApplicationUserManager userManager;

        public PostsAjaxController(
            ICategoryService categoryService,
            IPostService postService,
            ApplicationUserManager userManager)
        {
            this.categoryService = categoryService;
            this.postService = postService;
            this.userManager = userManager;
        }

        // POST: /Posts/Vote {voteType, postId}
        [HttpPost]
        [Authorize]
        public ActionResult Vote(Guid postId, VoteType voteType)
        {
            var enumIsValid = Enum.IsDefined(typeof(VoteType), voteType) && voteType != VoteType.None;
            var post = this.postService.GetById(postId).FirstOrDefault();

            if (!enumIsValid || post == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = this.User.Identity.GetUserId();
            this.postService.ToggleVote(postId, userId, voteType);

            return Json(new
            {
                success = true,
                likes = post.VoteCount.ToMetric()
            });
        }

        // POST: /Posts/Edit {postId, newContent}
        [HttpPost]
        [Authorize]
        [ValidateInput(false)]
        public ActionResult Edit(Guid postId, string content)
        {
            var post = this.postService.GetById(postId).FirstOrDefault();
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

        // POST: /Posts/Delete {postId, newContent}
        [HttpPost]
        [Authorize]
        public ActionResult Delete(Guid postId)
        {
            var post = this.postService.GetById(postId).FirstOrDefault();
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