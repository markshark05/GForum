using System;
using GForum.Data.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace GForum.Web.IdentityConfig
{
    public class ApplicationSignInManager : SignInManager<ApplicationUser, String>
    {
        public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
    }
}
