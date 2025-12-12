using System.Web;
using System.Web.Optimization;

namespace AutoSherpa_project
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                  "~/public/javascripts/jquery.js",
                  "~/public/javascripts/jquery-ui.min.js",
                  "~/public/javascripts/bootstrap.min.js",
                  "~/Content/master/dist/js/master.js",
                  "~/public/javascripts/jquery.blockUI.js",
                  "~/public/javascripts/multiselect.js",
                  "~/public/javascripts/Chart.min.js",
                  "~/public/javascripts/fastclick.min.js",
                  "~/public/javascripts/lobibox.js",
                  "~/public/javascripts/datatables.min.js",
                  "~/public/javascripts/jquery.ui.position.min.js",
                  "~/public/javascripts/jquery.ui.timepicker.js",
                  "~/public/javascripts/jquery.bootstrap.wizard.js",
                  "~/public/javascripts/wizard.js",
                  "~/public/javascripts/jquery.datetimepicker.full.js",
                  "~/public/javascripts/jquery.datetimepicker.js",
                   "~/Scripts/jquery.unobtrusive-ajax.min.js"
                        ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                  "~/public/css/jquery-ui.min.css",
                  "~/public/css/bootstrap.min.css",
                  "~/Content/master/dist/css/skin-blue.css",
                  "~/public/css/AdminLTE.min.css",
                  "~/public/css/dataTables.bootstrap.css",
                  "~/public/css/dataTables.responsive.css",
                  "~/public/css/font-awesome.min.css",
                  "~/public/css/CutomizedNewIconCSS.css",
                       "~/public/css/ionicons.min.css",
                       "~/public/css/animate.css",
                        "~/public/css/lobibox.min.css",
                        "~/public/css/Wyz_css.css",
                        "~/public/css/multiselect.css",
                        "~/public/css/jquery.datetimepicker.css"
                 ));


            bundles.Add(new Bundle("~/bundles/liveRptJqury").IncludeDirectory(@"~/public/liveReportjs", "*.js"));

            bundles.Add(new StyleBundle("~/bundles/CallLoggingCSS").Include(
                    "~/public/css/cre.css",
                    "~/public/CallLogFiles/jquery-confirm.min.css"
            ));

            bundles.Add(new ScriptBundle("~/bundles/CallLoggingJS").Include(
                "~/public/customer360JS/insuranceoutboundJS.js",
                "~/public/customer360JS/AjaxCallScriptsJS.js",
                "~/public/customer360JS/jquery-confirm.min.js"
                 ));
            bundles.Add(new ScriptBundle("~/bundles/BlueIMPFiles").Include(
                "~/public/BlueIMPFiles/tmpl.min.js",
                "~/public/BlueIMPFiles/load-image.all.min.js",
                "~/public/BlueIMPFiles/canvas-to-blob.min.js",
                "~/public/BlueIMPFiles/jquery.blueimp-gallery.min.js",
                "~/public/js/jquery.iframe-transport.js",
                "~/public/js/vendor/jquery.ui.widget.js",
                "~/public/js/jquery.fileupload.js",
                "~/public/js/jquery.fileupload-process.js",
                "~/public/js/jquery.fileupload-image.js",
                "~/public/js/jquery.fileupload-audio.js",
                "~/public/js/jquery.fileupload-video.js",
                "~/public/js/jquery.fileupload-validate.js",
                "~/public/js/jquery.fileupload-ui.js",
                "~/public/js/main.js"
                ));
        }

    }
}
