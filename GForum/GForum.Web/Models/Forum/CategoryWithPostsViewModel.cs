using System;
using System.Collections.Generic;

namespace GForum.Web.Models.Forum
{
    public class CategoryWithPostsViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}