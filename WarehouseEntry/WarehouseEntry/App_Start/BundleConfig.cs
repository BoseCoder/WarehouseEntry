using System.Web.Optimization;

namespace WarehouseEntry
{
    public class BundleConfig
    {
        // 有关 Bundling 的详细信息，请访问 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scriptcore").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/bootstrap-switch.js",
                        "~/Scripts/bootstrap-datetimepicker.js",
                        "~/Scripts/bootstrap-datetimepicker.zh-CN.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/plus").Include(
                        "~/Scripts/stickUp.js",
                        "~/Scripts/icheck.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom").Include(
                        "~/Scripts/custom.js"));

            // 使用要用于开发和学习的 Modernizr 的开发版本。然后，当你做好
            // 生产准备时，请使用 http://modernizr.com 上的生成工具来仅选择所需的测试。
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/CssCore/Bootstrap").Include(
                        "~/Content/Bootstrap/bootstrap.css",
                        "~/Content/Bootstrap/bootstrap-theme.css",
                        //"~/Content/Bootstrap/font-awesome-ie7.css",
                        "~/Content/Bootstrap/font-awesome.css",
                        "~/Content/Custom/Site.css"));

            bundles.Add(new StyleBundle("~/Content/CssPlus/BootstrapPlus").Include(
                        "~/Content/BootstrapPlus/bootstrap-buttons.css",
                        "~/Content/BootstrapPlus/bootstrap-datetimepicker.css",
                        "~/Content/BootstrapPlus/bootstrap-stickup.css",
                        "~/Content/BootstrapPlus/bootstrap-switch.css",
                        "~/Content/BootstrapPlus/Bootstrap-iCheck.css"));
        }
    }
}