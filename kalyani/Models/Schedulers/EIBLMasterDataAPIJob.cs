using Newtonsoft.Json.Linq;
using Quartz;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System;
using NLog;
using System.Configuration;
using AutoSherpa_project.Models.EIBL_API;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Policy;
using System.Web.Mvc;
using static Google.Apis.Requests.BatchRequest;

namespace AutoSherpa_project.Models.Schedulers
{
    public class EIBLMasterDataAPIJob : schedulerCommonFunction, IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            string siteRoot = HttpRuntime.AppDomainAppVirtualPath;

            logger.Info($"============= Get Master Location Data API Job Started ============= | AT: {DateTime.Now}");
            logger.Info($"Application Site Root: {siteRoot}");
            //if (siteRoot != "/")
            //{
            try
            {
                string URL = ConfigurationManager.AppSettings["GetMasterLocData_ApiUrl"].ToString();
                var accessToken = await GenerateToken.GenerateTokenAsync();

                using (var db = new AutoSherDBContext())
                {
                    using (var client = new HttpClient())
                    {
                        startScheduler("GetMasterLocationAPI_Scheduler");

                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

                        HttpResponseMessage responseContent = await client.PostAsync(URL, null);

                        string jsonResponseContent = await responseContent.Content.ReadAsStringAsync();
                        if (responseContent.IsSuccessStatusCode)
                        {
                            if (jsonResponseContent.Contains("ErrorCode") && jsonResponseContent.Contains("ErrorMessage"))
                            {
                                JObject jsonResponseObj = JObject.Parse(jsonResponseContent);
                                string errorMessage = jsonResponseObj["ErrorMessage"]?.ToString();
                                string errorCode = jsonResponseObj["ErrorCode"]?.ToString();

                                logger.Error($"[ERROR] Error While Fetching Data | AT: {DateTime.Now}\n" +
                                     $"Error Code: {errorCode}\n" +
                                     $"Error Message: {errorMessage}");
                            }
                            else
                            {
                                MasterStateCityPincodeVM responseDataObject = JsonConvert.DeserializeObject<MasterStateCityPincodeVM>(jsonResponseContent);
                                if (responseDataObject != null)
                                {
                                    if (responseDataObject.StateList?.Any() == true)
                                    {
                                        await db.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE EIBL_MasterState");

                                        var states = responseDataObject.StateList.Select(state => $"('{state.STATE_ID}','{state.STATE_NAME}','{state.STATE_CODE}')").ToList();

                                        if (states.Any())
                                        {
                                            string insertMasterStatesQuery = "INSERT INTO EIBL_MasterState(STATE_ID, STATE_NAME, STATE_CODE) VALUES" + string.Join(",", states);

                                            db.Database.ExecuteSqlCommand(insertMasterStatesQuery);
                                        }
                                    }

                                    if (responseDataObject.CityList.Any() == true)
                                    {
                                        await db.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE EIBL_MasterCity");

                                        var cities = responseDataObject.CityList.Select(city => $"('{city.CITY_CODE}','{city.CITY_NAME}','{city.FKSTATE_ID}','{city.CITY_ID}')").ToList();

                                        if (cities.Any())
                                        {
                                            string insertMasterCityQuery = "INSERT INTO EIBL_MasterCity(CITY_CODE, CITY_NAME, FKSTATE_ID, CITY_ID) VALUES " + string.Join(",", cities);

                                            db.Database.ExecuteSqlCommand(insertMasterCityQuery);
                                        }
                                    }

                                    if (responseDataObject.PIN_CODE_List?.Any() == true)
                                    {
                                        await db.Database.ExecuteSqlCommandAsync("TRUNCATE TABLE EIBL_MasterPincode");

                                        var pincodes = responseDataObject.PIN_CODE_List.Select(pincode => $"('{pincode.PINCODE}','{pincode.FKSTATE_ID}')").ToList();

                                        if (pincodes.Any())
                                        {
                                            string insertMasterPincodeQuery = "INSERT INTO EIBL_MasterPincode(PINCODE, FKSTATE_ID) VALUES " + string.Join(",", pincodes);

                                            db.Database.ExecuteSqlCommand(insertMasterPincodeQuery);
                                        }
                                    }

                                    logger.Info($"[SUCCESS] Get Master Location Data Job API Call Success: Master Data Fetched and Saved Successfully | AT: {DateTime.Now}\n" +
                                     $"Response Status Code: {responseContent.StatusCode} \n" +
                                     $"States Data count: {responseDataObject.StateList.Count} \n" +
                                     $"Cities Data count: {responseDataObject.CityList.Count} \n" +
                                     $"Pincode Data count: {responseDataObject.PIN_CODE_List.Count}");
                                }
                                else
                                {
                                    logger.Error($"[ERROR] Get Master Location Data Job API Call Failed: Response is Empty, No Data Found. | AT: {DateTime.Now}\n" +
                                   $"Response Status Code: {responseContent.StatusCode}");
                                }
                            }
                        }
                        else
                        {
                            JObject jsonDataObj = JObject.Parse(jsonResponseContent);

                            if (jsonDataObj.ContainsKey("Message"))
                            {
                                logger.Error($"[ERROR] Get Master Location Data Job API Call Failed: Response not Success | AT: {DateTime.Now}\n" +
                                     $"Response Status Code: {responseContent.StatusCode}\n" +
                                     $"Response Message: {jsonDataObj["Message"]}");
                            }

                            logger.Error($"[ERROR] Get Master Location Data Job API Call Failed: Response not Success | AT: {DateTime.Now}\n" +
                                     $"Response Status Code: {responseContent.StatusCode}\n" +
                                     $"Response Message: {jsonResponseContent}");
                        }
                        stopScheduler("GetMasterLocationAPI_Scheduler");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null)
                {
                    if (ex.InnerException.InnerException != null)
                    {
                        logger.Error($"[ERROR] Get Master Location Data Job API Call Failed | AT: {DateTime.Now}\n" +
                                     $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                     $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                     $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                    }
                    else
                    {
                        logger.Error($"[ERROR] Get Master Location Data Job API Call Failed | AT: {DateTime.Now}\n" +
                                    $"Error: {ex.InnerException.Message}\n" +
                                    $"Method: {ex.InnerException.TargetSite}\n" +
                                    $"StackTrace: {ex.InnerException.StackTrace}");
                    }
                }
                else
                {
                    logger.Error($"[ERROR] Get Master Location Data Job API Call Failed | AT: {DateTime.Now}\n" +
                                $"Error: {ex.Message}\n" +
                                $"Method: {ex.TargetSite}\n" +
                                $"StackTrace: {ex.StackTrace}");

                }
                //}
                stopScheduler("GetMasterLocationAPI_Scheduler");

                logger.Info($"============= Get Master Location Data API Job Stopped ============= | AT: {DateTime.Now}\n\n");
            }
        }
    }
}