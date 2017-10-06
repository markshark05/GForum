using System;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using GForum.Common;
using GForum.Data.Models;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Forum
{
    public class PostSubmitViewModel : IMapFrom<Category>, IHaveCustomMappings
    {
        public Guid CategoryId { get; set; }

        public string CategoryTitle { get; set; }

        [Required]
        [StringLength(Globals.PostTitleLength)]
        public string Title { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Category, PostSubmitViewModel>()
                .ForMember(d => d.CategoryId, opt => opt.MapFrom(s => s.Id))
                .ForMember(d => d.CategoryTitle, opt => opt.MapFrom(s => s.Title))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}