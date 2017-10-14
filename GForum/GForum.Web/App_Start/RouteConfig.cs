using System.Web.Mvc;
using System.Web.Routing;

namespace GForum.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.LowercaseUrls = true;

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "UserProfile",
                url: "Users/{username}",
                defaults: new { controller = "Users", action = "Profile" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "GForum.Web.Controllers" }
            );
        }
    }
}
