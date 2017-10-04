using System;

namespace GForum.Web.Models.Forum
{
    public class CategoryViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int PostsCount { get; set; }

        public AuthorViewModel Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}