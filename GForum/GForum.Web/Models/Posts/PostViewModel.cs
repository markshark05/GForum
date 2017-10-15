using System;
using GForum.Common.Enums;
using GForum.Data.Models;
using GForum.Web.Models.Shared;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Posts
{
    public class PostViewModel : IMapFrom<Post>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int VoteCount { get; set; }

        public VoteType UserVoteType { get; set; }

        public AuthorViewModel Author { get; set; }

        public DateTime CreatedOn { get; set; }
    }
}