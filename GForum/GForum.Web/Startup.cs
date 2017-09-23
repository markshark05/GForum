using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(GForum.Web.Startup))]
namespace GForum.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app) => this.ConfigureAuth(app);
    }
}
