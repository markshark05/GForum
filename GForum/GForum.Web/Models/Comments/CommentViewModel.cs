using System;
using GForum.Data.Models;
using GForum.Web.Models.Shared;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Comments
{
    public class CommentViewModel: IMapFrom<Comment>
    { 
        public string Content { get; set; }

        public AuthorViewModel Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}