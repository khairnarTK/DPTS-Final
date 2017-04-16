using System.Web.Optimization;

namespace DPTS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                "~/Content/wp-includes/js/jquery/jqueryb8ff.js",
                "~/Content/wp-includes/js/jquery/jquery-migrate.min330a.js",
                "~/Content/wp-includes/js/plupload/plupload.full.mincc91.js",
                "~/Content/wp-content/themes/docdirect/js/countdown1bba.js?ver=3.5",
                "~/Content/wp-content/plugins/unyson/framework/static/js/fw-form-helpers1c9b.js",
                "~/Content/wp-content/plugins/docdirect_core/js/functionsd5f7.js",
                "~/Content/wp-content/themes/docdirect/js/vendor/bootstrap.min1bba.js",
                "~/Content/wp-includes/js/jquery/ui/core.mine899.js",
                "~/Content/wp-includes/js/jquery/ui/widget.mine899.js",
                "~/Content/wp-includes/js/jquery/ui/mouse.mine899.js",
                "~/Content/wp-includes/js/jquery/ui/sortable.mine899.js",
                "~/Content/wp-includes/js/jquery/ui/slider.mine899.js",
                "~/Content/wp-includes/js/underscore.min4511.js",
                "~/Content/wp-includes/js/wp-util.min1c9b.js",
              //"~/Content/wp-content/themes/docdirect/js/docdirect_functions1bba.js",
                "~/Content/wp-content/themes/docdirect/js/moment1bba.js",
                //  "~/Content/wp-content/themes/docdirect/js/bookings1bba.js",
                "~/Content/wp-content/themes/docdirect/js/parallax1bba.js",
              //"~/Content/wp-content/themes/docdirect/js/prettyPhoto1bba.js",
            //  "~/Content/wp-content/themes/docdirect/js/user_profile1bba.js",

                "~/Content/wp-content/themes/docdirect/js/appear1bba.js",
              //  "~/Content/wp-content/themes/docdirect/js/countTo1bba.js",
              //  "~/Content/wp-content/themes/docdirect/js/sticky_message1bba.js",
                "~/Content/wp-content/themes/docdirect/js/vendor/modernizr-2.8.3-respond-1.4.2.min1bba.js",
                "~/Content/wp-content/themes/docdirect/js/chosen.jquery1bba.js",
               // "~/Content/wp-content/plugins/unyson/framework/extensions/shortcodes/shortcodes/section/static/js/jquery.fs.wallpaper1c9b.js",
                //"~/Content/wp-content/plugins/unyson/framework/extensions/shortcodes/shortcodes/section/static/js/scripts1c9b.js",
                "~/Content/wp-includes/js/wp-embed.min1c9b.js",
                //"~/Content/wp-content/themes/docdirect/js/map/markerclusterer.min1bba.js"ffileupload,
                // "~/Content/wp-content/themes/docdirect/js/map/infobox1bba.js",
                // "~/Content/wp-content/themes/docdirect/js/map/map1bba.js",
                //"~/Content/wp-content/themes/docdirect/js/docdirect_functions1bba.js",
                "~/Content/wp-content/themes/docdirect/js/datetimepicker1bba.js"
                //"~/Content/wp-content/themes/docdirect/js/docdir_maps1c9b.js"));
            //      "~/Content/Admin/js/bootstrap-datetimepicker.js"));
            ));


            bundles.Add(new StyleBundle("~/Content/wp-content/themes").Include(
                "~/Content/wp-content/themes/docdirect/css/bootstrap.min1bba.css",
                "~/Content/wp-content/themes/docdirect/css/font-awesome.min1bba.css",
                "~/Content/wp-content/themes/docdirect/css/normalize1bba.css",
                "~/Content/wp-content/themes/docdirect/css/icomoon1bba.css",
                "~/Content/wp-content/themes/docdirect/css/owl.theme1bba.css",
                //"~/Content/wp-content/themes/docdirect/css/prettyPhoto1bba.css",
                "~/Content/wp-content/themes/docdirect/style1bba.css",
                "~/Content/wp-content/themes/docdirect/css/typo1bba.css",
                "~/Content/wp-content/themes/docdirect/css/transitions1bba.css",
                "~/Content/wp-content/themes/docdirect/css/responsive1bba.css",
                "~/Content/wp-content/themes/docdirect/css/jquery-ui1bba.css",
                "~/Content/wp-content/themes/docdirect/css/color1bba.css",
                "~/Content/wp-content/themes/docdirect/css/datetimepicker1bba.css",
                "~/Content/wp-content/themes/docdirect/css/chosen1bba.css",
                "~/Content/wp-content/themes/docdirect/framework-customizations/extensions/breadcrumbs/static/css/style1c9b.css",
                "~/Content/wp-content/plugins/unyson/framework/extensions/builder/static/css/frontend-grid578f.css",
                "~/Content/wp-content/plugins/unyson/framework/extensions/forms/static/css/frontenda23f.css",
                "~/Content/wp-content/plugins/unyson/framework/extensions/shortcodes/shortcodes/section/static/css/jquery.fs.wallpaper1c9b.css",
                "~/Content/wp-content/plugins/unyson/framework/extensions/shortcodes/shortcodes/section/static/css/styles1c9b.css",
                "~/Content/wp-content/plugins/unyson/framework/extensions/shortcodes/shortcodes/button/static/css/styles1c9b.css",
                "~/Content/wp-content/plugins/unyson/framework/extensions/shortcodes/shortcodes/call-to-action/static/css/styles1c9b.css",
                "~/Content/loader.css",
                "~/Content/wp-content/themes/docdirect/customdpts.css"
                ));
#if DEBUG
            BundleTable.EnableOptimizations = false;
#else
        BundleTable.EnableOptimizations = true;
#endif
        }
    }
}