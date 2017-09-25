using System;

namespace GForum.Data.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual Category Category { get; set; }

        public virtual ApplicationUser Author { get; set; }
    }
}
