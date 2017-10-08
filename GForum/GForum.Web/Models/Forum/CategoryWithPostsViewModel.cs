using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GForum.Data.Models;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Forum
{
    public class CategoryWithPostsViewModel: IMapFrom<Category>, IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<PostViewModel> Posts { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<Category, CategoryWithPostsViewModel>()
                .ForMember(d => d.Posts, 
                    opt => opt.MapFrom(s => s.Posts
                        .Where(x => !x.IsDeleted)
                        .OrderByDescending(x => x.CreatedOn)));
        }
    }
}