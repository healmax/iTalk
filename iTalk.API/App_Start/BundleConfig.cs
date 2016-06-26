using System.Web;
using System.Web.Optimization;

namespace iTalk.API {
    public class BundleConfig {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles) {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js",
                "~/Scripts/jquery-ui-1.9.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/site.css",
                    "~/Content/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/bundles/unobtrusive").Include("~/Scripts/jquery.unobtrusive-ajax.js"));

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include("~/Scripts/angular.js",
                    "~/Scripts/angular-animate.js",
                    "~/Scripts/angular-aria.js",
                    "~/Scripts/angular-messages.js",
                    "~/Scripts/matchmedia-ng.js",
                    "~/Scripts/angular-material.js",
                    "~/Scripts/jquery.signalR-{version}.js",
                    "~/signalr/hubs"));

            bundles.Add(new ScriptBundle("~/bundles/angular-index")
                .IncludeDirectory("~/js", "*.js", false)
                .IncludeDirectory("~/js/controllers", "*.js", true)
                .IncludeDirectory("~/js/directives", "*.js", true)
                .IncludeDirectory("~/js/filters", "*.js", true)
                .IncludeDirectory("~/js/services", "*.js", true));

            bundles.Add(new StyleBundle("~/Content/angular").Include(
                    "~/Content/angular-material.css",
                    "~/Content/angular-material.layout.css"));
        }
    }
}
