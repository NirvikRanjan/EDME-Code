using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;
using System;
using NLog;

namespace AutoSherpa_project.Models.Schedulers
{
    public class EIBLMasterDataAPIJobScheduler
    {
        public static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;

        public static void InitializeSchedulerAsync()
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            try
            { 
                using(var db = new AutoSherDBContext())
                {
                    logger.Info("\n");
                    logger.Info($"============= Get Master Location Data Job Scheduler Initialization Started ============= | AT: {DateTime.Now}");

                    string cronSchedule = "0 30 6 ? * MON *"; // Every Monday at 6:30 AM
                    //string cronSchedule = "0 0 1 ? * MON *"; // According to UTC Time

                    //string cronSchedule = "0 0/10 * * * ? *";

                    scheduler.Start();

                    logger.Info("Scheduler started successfully.");

                    IJobDetail jobDetail = JobBuilder.Create<EIBLMasterDataAPIJob>().Build();

                    ITrigger trigger = TriggerBuilder.Create()
                        .WithIdentity("GetMasterLocationDataTrigger", "GetMasterLocationDataJobGroup")
                        .WithCronSchedule(cronSchedule)
                        .Build();

                    //ITrigger trigger = TriggerBuilder.Create()
                    //    .WithIdentity("GetMasterLocationDataTrigger", "GetMasterLocationDataJobGroup")
                    //    .StartNow()
                    //    .Build();

                    logger.Info($"Job scheduled with CRON expression: {cronSchedule}");

                    scheduler.ScheduleJob(jobDetail, trigger);

                    logger.Info($"============= Get Master Location Data Job Scheduler Initialization Completed ============= | AT: {DateTime.Now}\n");
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        logger.Error($"[ERROR]  Get Master Location Data Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                     $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                     $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                     $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                    }
                    else
                    {
                        logger.Error($"[ERROR]  Get Master Location Data Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                    $"Error: {ex.InnerException.Message}\n" +
                                    $"Method: {ex.InnerException.TargetSite}\n" +
                                    $"StackTrace: {ex.InnerException.StackTrace}");
                    }
                }
                else
                {
                    logger.Error($"[ERROR]  Get Master Location Data Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                 $"Error: {ex.Message}\n" +
                                 $"Method: {ex.TargetSite}\n" +
                                 $"StackTrace: {ex.StackTrace}");
                }
                logger.Info($"============= Get Master Location Data Job Scheduler Initialization Stopped ============= | AT: {DateTime.Now}\n");
            }
        }
    }
}