using System;

namespace GForum.Web.Models.Users
{
    public class IndexViewModel
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int PostsCount { get; set; }

        public int CategoriesCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}