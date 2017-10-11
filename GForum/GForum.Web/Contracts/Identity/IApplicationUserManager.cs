using System.Linq;
using System.Threading.Tasks;
using GForum.Data.Models;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Contracts.Identity
{
    public interface IApplicationUserManager
    {
        Task<ApplicationUser> FindByIdAsync(string userId);

        Task<IdentityResult> CreateAsync(ApplicationUser user, string password);

        Task<string> GetEmailAsync(string userId);

        Task<IdentityResult> SetEmailAsync(string userId, string email);

        Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

        IQueryable<ApplicationUser> Users { get; }
    }
}
