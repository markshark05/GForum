using System.Collections.Generic;
using GForum.Web.Models.Posts;

namespace GForum.Web.Models.Home
{
    public class HomeIndexViewModel
    {
        public IEnumerable<PostViewModel> RecentPosts { get; set; }

        public IEnumerable<PostViewModel> TopRatedPosts { get; set; }
    }
}