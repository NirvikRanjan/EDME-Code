using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSherpa_project.Models.Schedulers
{
    public class sqltoMysqlDataTransferAutoJOB
    {
        public static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        public static void InitializeScheduler()
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            try
            {
                //string cronSchedule = "0 0 11 1/1 * ? *";
                string cronSchedule = "0 0 3 1/1 * ? *";
                logger.Info("\n\n -------- Cron Job MySql To MsSql {0} : ",cronSchedule);

                scheduler.Start();

                IJobDetail jobDetail = JobBuilder.Create<sqltoMysqlDataTransferJOB>().Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartNow().WithCronSchedule(cronSchedule).Build();
                scheduler.ScheduleJob(jobDetail, trigger);
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        logger.Info("Cron Job MySql To MsSql {0} InnerException.InnerException.Message" + ex.InnerException.InnerException.Message);
                    }
                    else
                    {
                        logger.Info("Cron Job MySql To MsSql {0} InnerException.Message" + ex.InnerException.InnerException.Message);
                    }
                }
                else
                {
                    logger.Info("Cron Job MySql To MsSql {0} Message" + ex.Message);
                }
            }
        }
    }
}