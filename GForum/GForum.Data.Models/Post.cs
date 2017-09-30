using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GForum.Data.Models.Abstract;
using GForum.Data.Models.Contracts;

namespace GForum.Data.Models
{
    public class Post : Entity, IEntity
    {
        [Index]
        [Column(TypeName = "VARCHAR")]
        [StringLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public virtual ApplicationUser Author { get; set; }

        public virtual Category Category { get; set; }
    }
}
