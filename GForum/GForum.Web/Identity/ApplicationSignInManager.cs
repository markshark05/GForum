using System;
using GForum.Data.Models;
using GForum.Web.Contracts.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace GForum.Web.Identity
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, String>, IApplicationSignInManager
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
    }
}
