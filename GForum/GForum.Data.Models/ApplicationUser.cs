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
        }

        [Index]
        public bool IsDeleted { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CreatedOn { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedOn { get; set; }

        public virtual ICollection<Post> Posts { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
