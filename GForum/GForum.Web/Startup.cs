using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GForum.Startup))]
namespace GForum
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
