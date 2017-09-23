using GForum.Data.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GForum.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}