using System.Linq;
using GForum.Data.Models;

namespace GForum.Web.Areas.Admin.Models
{
    public class AdminPostsViewModel
    {
        public IQueryable<Post> PostsQueriable { get; set; }
    }
}