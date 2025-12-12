using AutoSherpa_project.Models.EIBL_API;
using Newtonsoft.Json;
using NLog;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AutoSherpa_project.Models.Schedulers
{
    public class GetPreviousPolicyNoAPIJob : schedulerCommonFunction, IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            string siteRoot = HttpRuntime.AppDomainAppVirtualPath;

            logger.Info("\n");
            logger.Info($"============= Get Previous Policy Number API Job Started ============= | AT: {DateTime.Now}");
            logger.Info($"Application Site Root: {siteRoot}");
            //if (siteRoot != "/")
            //{
                try
                {
                    string URL = ConfigurationManager.AppSettings["GetPreviousPolicyNo_ApiUrl"].ToString();

                    var accessToken = await GenerateToken.GenerateTokenAsync();

                    using (var db = new AutoSherDBContext())
                    {
                        var requestPayload = new
                        {
                            OEMID = 0,
                            SUBOEM_ID = 0
                        };

                        string jsonRequestPayload = JsonConvert.SerializeObject(requestPayload);

                        using(var httpClient = new HttpClient())
                        {
                            startScheduler("GetPreviousPolicyNoAPI_Scheduler");

                            var request = new HttpRequestMessage();
                            request.Method = HttpMethod.Post;
                            request.RequestUri = new Uri(URL);
                            request.Headers.Add("Authorization", "Bearer " + accessToken);
                            request.Content = new StringContent(jsonRequestPayload, Encoding.UTF8, "application/json");

                            HttpResponseMessage response = await httpClient.SendAsync(request);

                            var responseContent = await response.Content.ReadAsStringAsync();

                            if (response.IsSuccessStatusCode)
                            {
                                PreviousPolicyNoResponse previousPolicyNumbers = new PreviousPolicyNoResponse();

                                try
                                {
                                    previousPolicyNumbers = JsonConvert.DeserializeObject<PreviousPolicyNoResponse>(responseContent);
                                }
                                catch (JsonReaderException ex)
                                {
                                    if (ex.InnerException != null)
                                    {
                                        if (ex.InnerException.InnerException != null)
                                        {
                                            logger.Error($"[ERROR] Error Reading Get Previous Policy Number Job API Response: Json Serialize Exception | AT: {DateTime.Now}\n" +
                                                         $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                                         $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                                         $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                                        }
                                        else
                                        {
                                            logger.Error($"[ERROR] Error Reading Get Previous Policy Number Job API Response: Json Serialize Exception | AT: {DateTime.Now}\n" +
                                                        $"Error: {ex.InnerException.Message}\n" +
                                                        $"Method: {ex.InnerException.TargetSite}\n" +
                                                        $"StackTrace: {ex.InnerException.StackTrace}");
                                        }
                                    }
                                    else
                                    {
                                        logger.Error($"[ERROR] Error Reading Get Previous Policy Number Job API Response: Json Serialize Exception | AT: {DateTime.Now}\n" +
                                                    $"Error: {ex.Message}\n" +
                                                    $"Method: {ex.TargetSite}\n" +
                                                    $"StackTrace: {ex.StackTrace}");
                                    }
                                }
                                catch (Exception ex)
                                {
                                    if (ex.InnerException != null)
                                    {
                                        if (ex.InnerException.InnerException != null)
                                        {
                                            logger.Error($"[ERROR] Error Reading Get Previous Policy Number Job API Response: Json Serialize Exception | AT: {DateTime.Now}\n" +
                                                         $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                                         $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                                         $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                                        }
                                        else
                                        {
                                            logger.Error($"[ERROR] Error Reading Get Previous Policy Number Job API Response: Json Serialize Exception | AT: {DateTime.Now}\n" +
                                                        $"Error: {ex.InnerException.Message}\n" +
                                                        $"Method: {ex.InnerException.TargetSite}\n" +
                                                        $"StackTrace: {ex.InnerException.StackTrace}");
                                        }
                                    }
                                    else
                                    {
                                        logger.Error($"[ERROR] Error Reading Get Previous Policy Number Job API Response: Json Serialize Exception | AT: {DateTime.Now}\n" +
                                                    $"Error: {ex.Message}\n" +
                                                    $"Method: {ex.TargetSite}\n" +
                                                    $"StackTrace: {ex.StackTrace}");
                                    }
                                }

                                if (previousPolicyNumbers != null && (previousPolicyNumbers.Previous_Policy_No != null && previousPolicyNumbers.Previous_Policy_No.Count > 0))
                                {
                                    //List<PreviousPolicyNumbers> removePreviousPolicyNumbers = await db.PreviousPolicyNumbers.ToListAsync();
                                    //if (removePreviousPolicyNumbers != null && removePreviousPolicyNumbers.Count > 0)
                                    //{
                                    //    db.PreviousPolicyNumbers.RemoveRange(removePreviousPolicyNumbers);
                                    //    await db.SaveChangesAsync();
                                    //}

                                    List<PreviousPolicyNumbers> policyNumbers = new List<PreviousPolicyNumbers>();
                                    foreach (var policy in previousPolicyNumbers.Previous_Policy_No)
                                    {
                                        policyNumbers.Add(new PreviousPolicyNumbers
                                        {
                                            PolicyNo = policy.Policy_No
                                        });
                                    }

                                    db.PreviousPolicyNumbers.AddRange(policyNumbers);
                                    await db.SaveChangesAsync();

                                    logger.Info($"[SUCCESS] Previous Policy Numbers Fetched and Saved Successfully | AT: {DateTime.Now}\n" +
                                        $"Total PPN count recieved: {previousPolicyNumbers.Previous_Policy_No.Count}");
                                }
                                else
                                {
                                    logger.Info($"[INFO] Get Previous Policy Number Job API Response: No Data Found, PPN count is {previousPolicyNumbers.Previous_Policy_No.Count} | AT: {DateTime.Now}");
                                }
                            }
                            else
                            {
                                logger.Error($"[ERROR] Get Previous Policy Number Job API Call Failed: Response not Success | AT: {DateTime.Now}\n" +
                                         $"Response Status Code: {response.StatusCode}\n" +
                                         $"Response Message: {responseContent}");
                            }
                            stopScheduler("GetPreviousPolicyNoAPI_Scheduler");

                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            logger.Error($"[ERROR] Get Previous Policy Number Job API Call Failed: Main Exception | AT: {DateTime.Now}\n" +
                                         $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                         $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                         $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                        }
                        else
                        {
                            logger.Error($"[ERROR] Get Previous Policy Number Job API Call Failed: Main Exception | AT: {DateTime.Now}\n" +
                                        $"Error: {ex.InnerException.Message}\n" +
                                        $"Method: {ex.InnerException.TargetSite}\n" +
                                        $"StackTrace: {ex.InnerException.StackTrace}");
                        }
                    }
                    else
                    {
                        logger.Error($"[ERROR] Get Previous Policy Number Job API Call Failed: Main Exception | AT: {DateTime.Now}\n" +
                                    $"Error: {ex.Message}\n" +
                                    $"Method: {ex.TargetSite}\n" +
                                    $"StackTrace: {ex.StackTrace}");
                    }

                    stopScheduler("GetPreviousPolicyNoAPI_Scheduler");

                }
            //}
            logger.Info($"============= Get Previous Policy Number API Job Stopped ============= | AT: {DateTime.Now}\n");
        }
    }

    public class PreviousPolicyNoResponse
    {
        public List<PolicyNo> Previous_Policy_No { get; set; }
    }

    public class PolicyNo
    {
        public string Policy_No { get; set; }
    }
}