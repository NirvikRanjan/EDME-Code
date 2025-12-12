using AutoSherpa_project.Models.EIBL_API;
using Google.Apis.Auth.OAuth2.Responses;
using MySqlX.XDevAPI.Common;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AutoSherpa_project.Models.Schedulers
{
    public class renewalRateCard : schedulerCommonFunction, IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var accessToken = await GenerateToken.GenerateTokenAsync();

            Logger logger = LogManager.GetLogger("apkRegLogger");
            string siteRoot = HttpRuntime.AppDomainAppVirtualPath;

            logger.Info("\n");
            logger.Info($"============= Get Renewal Data API Started ============= | AT: {DateTime.Now}");
            logger.Info($"Application Site Root: {siteRoot}");
            //if (siteRoot != "/")
            //{
            using (var db = new AutoSherDBContext())
            {
                var client = new HttpClient();
                var url = ConfigurationManager.AppSettings["GetRenewalData_ApiUrl"];

                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                client.Timeout = TimeSpan.FromMinutes(3);

                try
                {
                    startScheduler("GetRenewalDataAPI_Scheduler");

                    var requestPayload = new
                    {
                        FromDate = "",
                        ToDate = "",
                        CPCode = "",
                        OEMID = 0,
                        Type = 0,
                        SUBOEM_ID = 0
                    };

                    string jsonRequestPayload = JsonConvert.SerializeObject(requestPayload);

                    StringContent requestContent = new StringContent(jsonRequestPayload, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(url, requestContent);

                    logger.Info($"[INFO] Renewal Policy Data API Response status {response.StatusCode} | AT: {DateTime.Now}");

                    response.EnsureSuccessStatusCode();

                    var responseContent = await response.Content.ReadAsStringAsync();

                    var newCallLogs = new List<ratecard>();
                    if (responseContent.TrimStart().StartsWith("["))
                    {
                        var jsonArray = JArray.Parse(responseContent);

                        foreach (var item in jsonArray)
                        {
                            var callDetails = item.ToObject<ratecard>();
                            newCallLogs.Add(callDetails);
                        }

                        db.ratecard.AddRange(newCallLogs);
                    }

                    try
                    {
                        await db.SaveChangesAsync();

                        if(newCallLogs.Count == 0)
                        {
                            logger.Info($"[INFO] Renewal Policy Data Count is Zero | AT: {DateTime.Now}\nRESPONSE: {responseContent}");
                        }
                        else
                        {
                            logger.Info($"[SUCCESS] Renewal Policy Data Count:{newCallLogs.Count} Successfully Fetched & Saved | AT: {DateTime.Now}");
                        } 
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"[ERROR] Failed to Save Renewal Data to Database | AT: {DateTime.Now}\n" +
                                     $"Exception: {ex.Message}\n" +
                                     $"Method: {ex.TargetSite}\n" +
                                     $"StackTrace: {ex.StackTrace}");

                        stopScheduler("GetRenewalDataAPI_Scheduler");
                    }

                    stopScheduler("GetRenewalDataAPI_Scheduler");
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            logger.Error($"[ERROR] Get Renewal Data Job API Call Failed | AT: {DateTime.Now}\n" +
                                         $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                         $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                         $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                        }
                        else
                        {
                            logger.Info($"[ERROR] Get Renewal Data Job API Call Failed | AT: {DateTime.Now}\n" +
                                        $"Error: {ex.InnerException.Message}\n" +
                                        $"Method: {ex.InnerException.TargetSite}\n" +
                                        $"StackTrace: {ex.InnerException.StackTrace}");
                        }
                    }
                    else
                    {
                        logger.Info($"[ERROR] Get Renewal Data Job API Call Failed | AT: {DateTime.Now}\n" +
                                    $"Error: {ex.Message}\n" +
                                    $"Method: {ex.TargetSite}\n" +
                                    $"StackTrace: {ex.StackTrace}");
                    }
                    stopScheduler("GetRenewalDataAPI_Scheduler");
                }
            }
            //}
            logger.Info($"============= Get Renewal Data API Stopped ============= | AT: {DateTime.Now}");
        }
    }

    public class RenewalApi
    {
        public static IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler().Result;
        public static void InitializeSchedulerAsync()
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            try
            {
                logger.Info("\n");
                logger.Info($"============= Renewal Data Job Scheduler Initialization Started ============= | AT: {DateTime.Now}");

                string cronSchedule = "0 30 19 * * ? *";
                //string cronSchedule = "0 30 19 * * ? *"; // Every Day at 7:30 PM

                scheduler.Start();

                logger.Info("[INFO] Scheduler started successfully.");

                IJobDetail jobDetail = JobBuilder.Create<renewalRateCard>().Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithIdentity("RenewalPostTrigger", "renewalPostJobGroup")
                    .WithCronSchedule(cronSchedule)
                    .Build();

                //ITrigger trigger = TriggerBuilder.Create()
                //    .WithIdentity("RenewalPostTrigger", "renewalPostJobGroup")
                //    .StartNow()
                //    .Build();

                logger.Info($"[INFO] Job scheduled with CRON expression: {cronSchedule}");

                scheduler.ScheduleJob(jobDetail, trigger);

                logger.Info($"============= Renewal Data Job Scheduler Initialization Completed ============= | AT: {DateTime.Now}\n");

            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        logger.Error($"[ERROR] Renewal Data Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                     $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                     $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                     $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                    }
                    else
                    {
                        logger.Error($"[ERROR] Renewal Data Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                    $"Error: {ex.InnerException.Message}\n" +
                                    $"Method: {ex.InnerException.TargetSite}\n" +
                                    $"StackTrace: {ex.InnerException.StackTrace}");
                    }
                }
                else
                {
                    logger.Error($"[ERROR] Renewal Data Job Scheduler Initialization Failed | AT: {DateTime.Now}\n" +
                                 $"Error: {ex.Message}\n" +
                                 $"Method: {ex.TargetSite}\n" +
                                 $"StackTrace: {ex.StackTrace}");
                }

                logger.Info($"============= Renewal Data Job Scheduler Initialization Stopped ============= | AT: {DateTime.Now}\n");
            }
        }
    }
}
