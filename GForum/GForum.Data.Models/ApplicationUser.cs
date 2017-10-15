using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using GForum.Data.Models.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GForum.Data.Models
{
    public class ApplicationUser : IdentityUser, IEntity
    {
        public ApplicationUser()
        {
            this.Posts = new HashSet<Post>();
            this.Categories = new HashSet<Category>();
            this.Comments = new HashSet<Comment>();
        }

        [Index]
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Category> Categories { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
    }
}
