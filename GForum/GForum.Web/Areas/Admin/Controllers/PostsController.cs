using System;
using System.Web.Mvc;
using GForum.Services.Contracts;
using GForum.Web.Areas.Admin.Models.Posts;

namespace GForum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        private readonly IPostService postService;

        public PostsController(IPostService postService)
        {
            this.postService = postService;
        }

        // GET: /Admin/Posts
        public ActionResult Index()
        {
            var model = new PostsViewModel
            {
                PostsQueriable = this.postService.GetAll(true),
            };

            return View(model);
        }

        // GET: /Admin/Posts/Delete
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Guid id)
        {
            this.postService.Delete(id);

            return RedirectToAction("Index");
        }

        // GET: /Admin/Posts/Restore
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Restore(Guid id)
        {
            this.postService.Restore(id);

            return RedirectToAction("Index");
        }
    }
}
