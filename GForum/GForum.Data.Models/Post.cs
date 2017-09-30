using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GForum.Common;
using GForum.Data.Models.Abstract;
using GForum.Data.Models.Contracts;

namespace GForum.Data.Models
{
    public class Post : Entity, IEntity
    {
        [Index]
        [Required]
        [StringLength(Globals.PostTitleLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public virtual ApplicationUser Author { get; set; }

        [Required]
        public virtual Category Category { get; set; }
    }
}
