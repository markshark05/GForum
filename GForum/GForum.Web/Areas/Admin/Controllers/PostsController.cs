using System;
using System.Web.Mvc;
using AutoMapper;
using GForum.Services.Contracts;
using GForum.Web.Areas.Admin.Models;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        private readonly IPostService postService;
        private readonly IMapper mapper;

        public PostsController(
            IPostService postService,
            IMapper mapper)
        {
            this.postService = postService;
            this.mapper = mapper;
        }

        // GET: /Admin/Categories
        public ActionResult Index()
        {
            var model = new AdminPostsViewModel
            {
                PostsQueriable = this.postService.GetAll(true),
            };

            return View(model);
        }

        //// GET: /Admin/Categories/Delete
        //[HttpPost]
        //[Authorize]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(Guid id)
        //{
        //    this.postService.Delete(id);

        //    return RedirectToAction("Index");
        //}
    }
}