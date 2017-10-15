using System;
using GForum.Common.Enums;
using GForum.Data.Models.Abstract;
using GForum.Data.Models.Contracts;

namespace GForum.Data.Models
{
    public class Vote: Entity, IEntity, IEntityWithGuid
    {
        public VoteType VoteType { get; set; }

        public Guid? PostId { get; set; } 
        public virtual Post Post { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}