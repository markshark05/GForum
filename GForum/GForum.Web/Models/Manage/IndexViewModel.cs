using System.Collections.Generic;
using Microsoft.AspNet.Identity;

namespace GForum.Web.Models.Manage
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }
        public IList<UserLoginInfo> Logins { get; set; }
        public bool BrowserRemembered { get; set; }
    }
}