using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;
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

        public string GetAvatar(int sizePx = 80)
        {
            using (var md5 = MD5.Create())
            {
                var url = "https://www.gravatar.com/avatar/{0}?s={1}&d=retro";
                var email = this.Email ?? string.Empty;
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return string.Format(url, sb, sizePx);
            }
        }
    }
}
