using System.Web;
using System.Web.Optimization;

namespace easyfis
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // ====================================
            // Library Cascading Style Sheets - CSS
            // ====================================
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/bootstrap.min.css"));

            // ====================================================
            // Library Cascading Style Sheets for Fontawesome - CSS
            // ====================================================
            bundles.Add(new StyleBundle("~/Font-Awesome/css").Include(
                      "~/font-awesome/css/font-awesome.css",
                      "~/font-awesome/css/font-awesome.min.css"));

            // ===================================
            // Custom Cascading Style Sheets - CSS
            // ===================================
            bundles.Add(new StyleBundle("~/Content/custom-css").Include(
                      "~/Content/style.css"));

            // ========================
            // Library Javascripts - JS
            // ========================
            bundles.Add(new ScriptBundle("~/Scripts/js").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/jquery.min.js",
                      "~/Scripts/collapse.js",
                      "~/Scripts/tooltip.js",
                      "~/Scripts/custom-tooltip.js",
                      "~/Scripts/dropdown.js",
                      "~/Scripts/transition.js",
                      "~/Scripts/scrollspy.js",
                      "~/Scripts/modal.js",
                      "~/Scripts/alert.js",
                      "~/Scripts/respond.js"));

            // =======================
            // Custom JavaScripts - JS
            // =======================
            bundles.Add(new ScriptBundle("~/Scripts/custom-js").Include(
                     "~/Scripts/style.js"));

            // ========
            // JQueries
            // ========
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));
            
            // =================
            // JQuery Validation
            // =================
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
        }
    }
}
