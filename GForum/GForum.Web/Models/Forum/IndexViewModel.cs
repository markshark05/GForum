using System;
using System.Collections.Generic;

namespace GForum.Web.Models.Forum
{
    public class ForumIndexViewModel
    {
        public class Author
        {
            public string Id { get; set; }

            public string UserName { get; set; }
        }

        public class Category
        {
            public Guid Id { get; set; }

            public string Title { get; set; }

            public int PostsCount { get; set; }

            public Author Author { get; set; }

            public DateTime CreatedOn { get; set; }
        }

        public IEnumerable<Category> Categories { get; set; }
    }
}