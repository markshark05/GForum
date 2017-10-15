using System;
using GForum.Data.Models.Abstract;
using GForum.Data.Models.Contracts;

namespace GForum.Data.Models
{
    public class Comment: Entity, IEntity, IEntityWithGuid
    {
        public string Content { get; set; }

        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public Guid? PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}