using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoSherpa_project.Models.Schedulers
{
    public class GetPreviousPolicyNoAPIJobScheduler
    {
        public static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

        public static void InitializeSchedulerAsync()
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");

            try
            {
                logger.Info("\n");
                logger.Info($"============= Get Previous Policy Number Job Scheduler Initialization Started ============= | AT: {DateTime.Now}");

                string cronSchedule = "0 30 7 * * ? *"; // Every Day at 7:30 AM
                //string cronSchedule = "0 0 2 * * ? *"; // Every Day at 7:30 AM According to UTC Time

                scheduler.Start();
                logger.Info("Scheduler started successfully.");

                IJobDetail jobDetail = JobBuilder.Create<GetPreviousPolicyNoAPIJob>().Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("GetPreviousPolicyNoTrigger", "GetPreviousPolicyNoTriggerGroup")
                    .WithCronSchedule(cronSchedule)
                    .Build();

                //            ITrigger trigger = TriggerBuilder.Create()
                //.WithIdentity("GetPreviousPolicyNoTrigger", "GetPreviousPolicyNoTriggerGroup")
                //.StartAt(DateBuilder.TodayAt(7, 0, 0)) // 6:00 AM today (or tomorrow if already past)
                //.WithSimpleSchedule(s => s
                //    .WithIntervalInHours(24) // Run every 24 hours
                //    .RepeatForever())
                //.Build();

                //ITrigger trigger = TriggerBuilder.Create()
                //    .WithIdentity("GetPreviousPolicyNoTrigger", "GetPreviousPolicyNoTriggerGroup")
                //    .StartNow()
                //    .Build();

                logger.Info($"Trigger and Job Details: \n{jobDetail} \n{trigger}");

                logger.Info($"Job scheduled with CRON expression: {cronSchedule}");

                scheduler.ScheduleJob(jobDetail, trigger);

                logger.Info($"============= Get Previous Policy Number Job Scheduler Initialization Completed ============= | AT: {DateTime.Now}\n");
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        logger.Error($"[ERROR]  Get Previous Policy Number Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                     $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                     $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                     $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                    }
                    else
                    {
                        logger.Error($"[ERROR]  Get Previous Policy Number Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                    $"Error: {ex.InnerException.Message}\n" +
                                    $"Method: {ex.InnerException.TargetSite}\n" +
                                    $"StackTrace: {ex.InnerException.StackTrace}");
                    }
                }
                else
                {
                    logger.Error($"[ERROR]  Previous Policy Number Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                 $"Error: {ex.Message}\n" +
                                 $"Method: {ex.TargetSite}\n" +
                                 $"StackTrace: {ex.StackTrace}");
                }
                logger.Info($"============= Get Previous Policy Number Job Scheduler Initialization Stopped ============= | AT: {DateTime.Now}\n");
            }
        }
    }
}