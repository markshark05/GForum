﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GForum.Common;
using GForum.Data.Models.Abstract;
using GForum.Data.Models.Contracts;

namespace GForum.Data.Models
{
    public class Post : Entity, IEntity, IEntityWithGuid
    {
        public Post()
        {
            this.Votes = new HashSet<Vote>();
            this.Comments = new HashSet<Comment>();
        }

        [Index]
        [Required]
        [StringLength(Globals.PostTitleLength)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public int VoteCount { get; set; }

        public int CommentCount { get; set; }

        public DateTime? EditedOn { get; set; }

        public string AuthorId { get; set; }
        public virtual ApplicationUser Author { get; set; }

        public Guid? CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Vote> Votes { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
