using System;
using System.Collections.Generic;
using GForum.Data.Models;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Forum
{
    public class CategoryWithPostsViewModel: IMapFrom<Category>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }
    }
}