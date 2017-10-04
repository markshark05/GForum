﻿using GForum.Data.Models;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Forum
{
    public class AuthorViewModel: IMapFrom<ApplicationUser>
    {
        public string UserName { get; set; }
    }
}