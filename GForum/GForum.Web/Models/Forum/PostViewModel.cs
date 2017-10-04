using System;

namespace GForum.Web.Models.Forum
{
    public class PostViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public AuthorViewModel Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}