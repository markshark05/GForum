using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using GForum.Services.Contracts;
using GForum.Web.Models.Home;
using GForum.Web.Models.Posts;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostService postService;
        private readonly IVoteService voteService;
        private readonly IMapper mapper;

        public HomeController(
            IPostService postService,
            IVoteService voteService,
            IMapper mapper)
        {
            this.postService = postService;
            this.voteService = voteService;
            this.mapper = mapper;
        }

        // GET: /
        public ActionResult Index()
        {
            var model = new HomeIndexViewModel
            {
                RecentPosts = this.postService
                    .GetRecent(5)
                    .Select(this.mapper.Map<PostViewModel>)
                    .ToList(),
                TopRatedPosts = this.postService
                    .GetTopRated(5)
                    .Select(this.mapper.Map<PostViewModel>)
                    .ToList()
            };

            if (this.Request.IsAuthenticated)
            {
                foreach (var post in model.RecentPosts)
                {
                    post.UserVoteType = this.voteService
                        .GetUserVoteTypeForPost(post.Id, this.User.Identity.GetUserId());
                }

                foreach (var post in model.TopRatedPosts)
                {
                    post.UserVoteType = this.voteService
                        .GetUserVoteTypeForPost(post.Id, this.User.Identity.GetUserId());
                }
            }

            return View(model);
        }

        // GET: /Home/Index
        public ActionResult About()
        {
            return View();
        }
    }
}