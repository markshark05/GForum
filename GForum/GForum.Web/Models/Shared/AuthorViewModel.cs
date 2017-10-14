using GForum.Data.Models;
using Heroic.AutoMapper;

namespace GForum.Web.Models.Shared
{
    public class AuthorViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string Email { get; set; }

        public string UserName { get; set; }
    }
}