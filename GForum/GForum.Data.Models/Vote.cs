using System;
using GForum.Common.Enums;
using GForum.Data.Models.Abstract;

namespace GForum.Data.Models
{
    public class Vote: Entity
    {
        public VoteType VoteType { get; set; }

        public Guid? PostId { get; set; } 
        public virtual Post Post { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}