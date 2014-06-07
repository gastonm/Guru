using System.Web;
using System.Web.Optimization;

namespace GolGuru
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {


            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-1.8.2.min*"));

            //bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include("~/Scripts/jquery-ui*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/functions").Include("~/Scripts/functions.js"));//C:\SourceCode\GolGuru\GolGuru\Scripts\functions.js

            bundles.Add(new ScriptBundle("~/bundles/jquerymobile").Include("~/Scripts/jquery.mobile*"));
            bundles.Add(new StyleBundle("~/Content/style").Include("~/Content/style.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            //bundles.Add(new StyleBundle("~/Content/mobilecss").Include("~/Content/jquery.mobile*"));

            bundles.IgnoreList.Clear();

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.min.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.min.js"));
            //    bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
            //                "~/Content/themes/base/jquery.ui.core.css",
            //                "~/Content/themes/base/jquery.ui.resizable.css",
            //                "~/Content/themes/base/jquery.ui.selectable.css",
            //                "~/Content/themes/base/jquery.ui.accordion.css",
            //                "~/Content/themes/base/jquery.ui.autocomplete.css",
            //                "~/Content/themes/base/jquery.ui.button.css",
            //                "~/Content/themes/base/jquery.ui.dialog.css",
            //                "~/Content/themes/base/jquery.ui.slider.css",
            //                "~/Content/themes/base/jquery.ui.tabs.css",
            //                "~/Content/themes/base/jquery.ui.datepicker.css",
            //                "~/Content/themes/base/jquery.ui.progressbar.css",
            //                "~/Content/themes/base/jquery.ui.theme.css"));
            //
        }
    }
}