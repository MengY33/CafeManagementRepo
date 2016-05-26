using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace CafeManagementWeb.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css", "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/scripts/modernizr-*"));
        }
    }
}