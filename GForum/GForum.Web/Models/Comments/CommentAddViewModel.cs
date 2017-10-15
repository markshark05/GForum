using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GForum.Data.Models;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Comments
{
    public class CommentAddViewModel: IMapFrom<Post>, IHaveCustomMappings
    {
        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        [Required]
        public Guid PostId { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Post, CommentAddViewModel>()
                 .ForMember(d => d.PostId, opt => opt.MapFrom(s => s.Id))
                 .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}