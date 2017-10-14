using System;
using System.Linq;
using AutoMapper;
using GForum.Data.Models;
using GForum.Web.Models.Shared;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Categories
{
    public class CategoryViewModel: IMapFrom<Category>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public int PostsCount { get; set; }

        public AuthorViewModel Author { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Category, CategoryViewModel>()
                .ForMember(d =>
                    d.PostsCount, opt =>
                        opt.MapFrom(s => 
                            s.Posts.Where(x => !x.IsDeleted).Count()));
        }
    }
}