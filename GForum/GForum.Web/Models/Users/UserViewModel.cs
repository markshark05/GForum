using System;
using GForum.Data.Models;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Users
{
    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int PostsCount { get; set; }

        public int CategoriesCount { get; set; }

        public int CommentsCount { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}