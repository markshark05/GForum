using System.Threading.Tasks;
using GForum.Data.Models;
using Microsoft.AspNet.Identity.Owin;

namespace GForum.Web.Contracts.Identity
{
    public interface IApplicationSignInManager
    {
        Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout);

        Task SignInAsync(ApplicationUser user, bool isPersistent, bool rememberBrowser);
    }
}
