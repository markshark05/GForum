using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNet.Identity.EntityFramework;

namespace GForum.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string GetAvatar(int sizePx = 80)
        {
            using (var md5 = MD5.Create())
            {
                var url = "https://www.gravatar.com/avatar/{0}?s={1}&d=retro";
                var email = this.Email ?? string.Empty;
                var bytes = md5.ComputeHash(Encoding.UTF8.GetBytes(email));
                var sb = new StringBuilder();
                foreach (var b in bytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return string.Format(url, sb, sizePx);
            }
        }
    }
}