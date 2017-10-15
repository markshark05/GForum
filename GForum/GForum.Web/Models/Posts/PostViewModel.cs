using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GForum.Common.Enums;
using GForum.Data.Models;
using GForum.Web.Models.Comments;
using GForum.Web.Models.Shared;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Posts
{
    public class PostViewModel : IMapFrom<Post>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public int VoteCount { get; set; }

        public VoteType UserVoteType { get; set; }

        public AuthorViewModel Author { get; set; }

        public Guid CategoryId { get; set; }

        public string CategoryTitle { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? EditedOn { get; set; }

        public CommentAddViewModel AddComment { get; set; }

        public IEnumerable<CommentViewModel> Comments { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Post, PostViewModel>()
                .ForMember(d => d.Comments, opt => opt.MapFrom(s => s.Comments.OrderBy(x => x.CreatedOn)))
                .ForMember(d => d.AddComment, opt => opt.MapFrom(s => s));
        }
    }
}