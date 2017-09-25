using System;
using System.Collections.Generic;

namespace GForum.Data.Models
{
    public class Category
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateCreated { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}