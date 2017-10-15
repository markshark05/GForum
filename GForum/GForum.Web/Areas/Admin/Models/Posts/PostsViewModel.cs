using System.Linq;
using GForum.Data.Models;

namespace GForum.Web.Areas.Admin.Models.Posts
{
    public class PostsViewModel
    {
        public IQueryable<Post> PostsQueriable { get; set; }
    }
}