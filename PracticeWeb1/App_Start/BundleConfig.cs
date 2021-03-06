﻿using System.Web;
using System.Web.Optimization;

namespace PracticeWeb1
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js",
                      "~/Scripts/toastr.js"));

            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
                      "~/Scripts/productImagePreview.js",
                      "~/Scripts/productIndexView.js",
                      "~/Scripts/dropzone/dropzone.js",
                      "~/Scripts/jquery.fancybox.js",
                      "~/Scripts/categoryIndexView.js",
                      "~/Scripts/adminMenu.js",
                      "~/Scripts/bootbox.js",
                      "~/Scripts/logoff.js",
                      "~/Scripts/Cart/addToCart.js",
                      "~/Scripts/Cart/clearCart.js",
                      "~/Scripts/Cart/incProduct.js",
                      "~/Scripts/Order/OrderScript.js",
                      "~/Scripts/UserAccount/userAccountScript.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/Order/OrderStyles"));

            bundles.Add(new StyleBundle("~/Content/UI").Include(
                      "~/Content/searchForm.css",
                      "~/Scripts/dropzone/dropzone.css",
                      "~/Scripts/dropzone/basic.css",
                      "~/Content/jquery.fancybox.css",
                      "~/Content/Categories.css",
                      "~/Content/CategoryThumbnail.css",
                      "~/Content/ProductThumbnail.css"));
        }
    }
}
