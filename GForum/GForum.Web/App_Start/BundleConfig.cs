using System.Web.Optimization;

namespace GForum.Web
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/toastr").Include(
                      "~/Scripts/toastr.js",
                      "~/Scripts/Site/toastr-config.js"));

            bundles.Add(new StyleBundle("~/Content/Styles/all").Include(
                      "~/Content/Styles/bootstrap.css",
                      "~/Content/Styles/toastr.css",
                      "~/Content/Styles/site.css"));
        }
    }
}
