using System;
using GForum.Common.Enums;
using GForum.Data.Models;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Forum
{
    public class PostViewModel : IMapFrom<Post>
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int VoteCount { get; set; }

        public VoteType UserVoteType { get; set; }

        public AuthorViewModel Author { get; set; }

        public Guid CategoryId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EditedOn { get; set; }
    }
}