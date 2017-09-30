using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Title { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
    }
}