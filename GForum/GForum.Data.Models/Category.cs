using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GForum.Common;
using GForum.Data.Models.Abstract;
using GForum.Data.Models.Contracts;

namespace GForum.Data.Models
{
    public class Category: Entity, IEntity
    {
        public Category()
        {
            this.Posts = new HashSet<Post>();
        }

        [Index]
        [Required]
        [StringLength(Globals.CategoryTitleLength)]
        public string Title { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}