using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace AutoSherpa_project.Models.Schedulers
{
    public class OnlinePaymentVerificationAPIJobScheduler
    {
        public static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

        public static void InitializeSchedulerAsync()
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            try
            {
                using (var db = new AutoSherDBContext())
                {
                    logger.Info("\n");
                    logger.Info($"============= Online Payment Verification Job Scheduler Initialization Started ============= | AT: {DateTime.Now}");

                    string cronSchedule = "0 0 7 * * ? *";
                    //string cronSchedule = "0 30 1 * * ? *"; // Every Day at 7 AM According to UTC Time

                    scheduler.Start();

                    logger.Info("Scheduler started successfully.");

                    IJobDetail jobDetail = JobBuilder.Create<OnlinePaymentVerificationAPIJob>().Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("OnlinePaymentVerificationTrigger", "OnlinePaymentVerificationJobGroup")
                        .WithCronSchedule(cronSchedule)
                        .Build();

                    //ITrigger trigger = TriggerBuilder.Create()
                    //    .WithIdentity("OnlinePaymentVerificationTrigger", "OnlinePaymentVerificationJobGroup")
                    //    .StartNow()
                    //    .Build();

                    //                ITrigger trigger = TriggerBuilder.Create()
                    //.WithIdentity("OnlinePaymentVerificationTrigger", "OnlinePaymentVerificationJobGroup")
                    //.StartAt(DateBuilder.TodayAt(7, 30, 0)) // 6:00 AM today (or tomorrow if already past)
                    //.WithSimpleSchedule(s => s
                    //    .WithIntervalInHours(24) // Run every 24 hours
                    //    .RepeatForever())
                    //.Build();

                    logger.Info($"Job scheduled with CRON expression: {cronSchedule}");

                    scheduler.ScheduleJob(jobDetail, trigger);

                    logger.Info($"============= Online Payment Verification Job Scheduler Initialization Completed ============= | AT: {DateTime.Now}\n");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        logger.Error($"[ERROR]  Online Payment Verification Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                     $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                     $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                     $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                    }
                    else
                    {
                        logger.Error($"[ERROR]  Online Payment Verification Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                    $"Error: {ex.InnerException.Message}\n" +
                                    $"Method: {ex.InnerException.TargetSite}\n" +
                                    $"StackTrace: {ex.InnerException.StackTrace}");
                    }
                }
                else
                {
                    logger.Error($"[ERROR]  Online Payment Verification Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                 $"Error: {ex.Message}\n" +
                                 $"Method: {ex.TargetSite}\n" +
                                 $"StackTrace: {ex.StackTrace}");
                }

                logger.Info($"============= Online Payment Verification Job Scheduler Initialization Stopped ============= | AT: {DateTime.Now}\n");
            }
        }
    }
}