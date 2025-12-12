using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoSherpa_project.Models;
using System.Globalization;
using System.Threading;
using AutoSherpa_project.Models.Schedulers;
using System.Configuration;
using AutoSherpa_project.Models.APIKey_Handler;
//using AutoSherpa_project.Migrations;

namespace AutoSherpa_project
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            Database.SetInitializer<AutoSherDBContext>(null);
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AutoSherDBContext, Configuration>());

            GlobalConfiguration.Configuration.MessageHandlers.Add(new APIkkeyHandler());
            CultureInfo newCulture = (CultureInfo)System.Threading.Thread.CurrentThread.CurrentCulture.Clone();
            newCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
            newCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            newCulture.DateTimeFormat.TimeSeparator = ":";
            newCulture.DateTimeFormat.DateSeparator = "-";
            Thread.CurrentThread.CurrentCulture = newCulture;
            //sqltoMysqlDataTransferAutoJOB.InitializeScheduler();
            FirebaseJob.InitializeScheduler();

            RenewalApi.InitializeSchedulerAsync();
            EIBLMasterDataAPIJobScheduler.InitializeSchedulerAsync();
            OnlinePaymentVerificationAPIJobScheduler.InitializeSchedulerAsync();
            GetPreviousPolicyNoAPIJobScheduler.InitializeSchedulerAsync();

        }
    }
}
