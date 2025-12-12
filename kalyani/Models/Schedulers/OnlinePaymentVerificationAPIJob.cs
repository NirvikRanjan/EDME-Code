using Quartz.Impl;
using Quartz;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using AutoSherpa_project.Models.EIBL_API;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Web;
using System.Configuration;
using System.Data.Entity;
using System.Runtime.Serialization;
using static Google.Apis.Requests.BatchRequest;
using System.Data.Entity.Migrations;
using System.Web.Mvc;
using System.Web.Routing;

namespace AutoSherpa_project.Models.Schedulers
{
    public class OnlinePaymentVerificationAPIJob : schedulerCommonFunction, IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            Logger logger = LogManager.GetLogger("apkRegLogger");
            string siteRoot = HttpRuntime.AppDomainAppVirtualPath;

            logger.Info("\n");
            logger.Info($"============= Online Payment Verification API Job Started ============= | AT: {DateTime.Now}");
            logger.Info($"Application Site Root: {siteRoot}");
            //if (siteRoot != "/")
            //{
            try
            {
                    string URL = ConfigurationManager.AppSettings["UpdateOnlinePayment_ApiUrl"].ToString();

                    var accessToken = await GenerateToken.GenerateTokenAsync();
                    DateTime tokenGeneratedTime = DateTime.Now;
                    TimeSpan tokenExpiryDuration = TimeSpan.FromMinutes(28);

                    using (var db = new AutoSherDBContext())
                    {

                        DateTime previousDayStartDateTime = DateTime.Now.AddDays(-1).Date;
                        DateTime previousDayEndDateTime = previousDayStartDateTime.AddDays(1).AddTicks(-1);

                        List<Quotationresponse> paymentVerificationDatas = await db.quotationresponses.Where(m => m.IsPaymentLinkSent == "Sent" && m.FinalQuoteStatus == "PENDING PAYMENT" && m.PayLinkSentDateTime >= previousDayStartDateTime && m.PayLinkSentDateTime <= previousDayEndDateTime).ToListAsync();

                        startScheduler("OnlinePaymentVerifyAPI_Scheduler");
                        if (paymentVerificationDatas.Count > 0 && paymentVerificationDatas != null)
                        {
                            using (var client = new HttpClient())
                            {
                                logger.Info($"============= [START] Online Payment Verification API Responses =============");

                                int respCount = 1;
                                foreach (var verifyPayData in paymentVerificationDatas)
                                {
                                    var dataToVerifyOnlinePayment = await db.quotationresponses.FirstOrDefaultAsync(q => q.id == verifyPayData.id);
                                    logger.Info($"Quote Data to Verify Online Payment for Proposal ID: {dataToVerifyOnlinePayment.KYCResponseProposalID} and Quote ID: {dataToVerifyOnlinePayment.KYCResponseQuoteID}");

                                    var requestPayload = new
                                    {
                                        ProposalId = dataToVerifyOnlinePayment.KYCResponseProposalID,
                                        QuotationId = dataToVerifyOnlinePayment.KYCResponseQuoteID
                                    };

                                    if ((DateTime.Now - tokenGeneratedTime) >= tokenExpiryDuration)
                                    {
                                        accessToken = await GenerateToken.GenerateTokenAsync();
                                        logger.Info($"[INFO]  Token Generated AT: {tokenGeneratedTime}, Token Expired and Refreshed AT: {DateTime.Now}.");
                                        tokenGeneratedTime = DateTime.Now;
                                    }

                                    string jsonRequestPayload = JsonConvert.SerializeObject(requestPayload);
                                    StringContent requestContent = new StringContent(jsonRequestPayload, Encoding.UTF8, "application/json");

                                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                                    HttpResponseMessage responseContent = await client.PostAsync(URL, requestContent);

                                    string jsonResponseContent = await responseContent.Content.ReadAsStringAsync();
                                    logger.Info($"[INFO]  {respCount}. {jsonResponseContent}");
                                    respCount++;

                                    if (responseContent.IsSuccessStatusCode)
                                    {
                                        List<PaymentStatusAPIResponse> paymentStatusResponse = new List<PaymentStatusAPIResponse>();
                                        try
                                        {
                                            paymentStatusResponse = JsonConvert.DeserializeObject<List<PaymentStatusAPIResponse>>(jsonResponseContent);
                                        }
                                        catch (JsonReaderException jsonEx)
                                        {
                                            logger.Error($"[ERROR] JSON Deserialization Failed | AT: {DateTime.Now}\n" +
                                                         $"Response: {jsonResponseContent}\n" +
                                                         $"Error: {jsonEx.Message}\n" +
                                                         $"StackTrace: {jsonEx.StackTrace}");
                                        }
                                        catch (Exception ex)
                                        {
                                            Exception rootCause = ex;
                                            while (rootCause.InnerException != null)
                                            {
                                                rootCause = rootCause.InnerException;
                                            }

                                            logger.Error($"[ERROR] Online Payment Verification Job API Call Failed: Exception Reading Response | AT: {DateTime.Now}\n" +
                                                         $"Error: {rootCause.Message}\n" +
                                                         $"Method: {rootCause.TargetSite}\n" +
                                                         $"StackTrace: {rootCause.StackTrace}");
                                        }

                                        if (paymentStatusResponse != null && paymentStatusResponse.Count > 0)
                                        {
                                            foreach (var paymentStatus in paymentStatusResponse)
                                            {
                                                dataToVerifyOnlinePayment.PaymentDoneDate = paymentStatus.Payment_Date ?? "";
                                                dataToVerifyOnlinePayment.PaymentStatus = paymentStatus.Status ?? "";
                                                dataToVerifyOnlinePayment.TransactionReferenceNumber = paymentStatus.Transaction_Reference_Number ?? "";

                                                if (paymentStatus.Status == "Success")
                                                {
                                                    dataToVerifyOnlinePayment.FinalQuoteStatus = "PAYMENT SUCCESS";
                                                }
                                                else if (paymentStatus.Status == "Failed")
                                                {
                                                    dataToVerifyOnlinePayment.FinalQuoteStatus = "PAYMENT EXPIRED";
                                                }
                                                else
                                                {
                                                    dataToVerifyOnlinePayment.FinalQuoteStatus = "";
                                                }
                                                dataToVerifyOnlinePayment.FinalStatusUpdatedDateTime = DateTime.Now;

                                                db.quotationresponses.AddOrUpdate(dataToVerifyOnlinePayment);

                                                PaymentStatusResponse paymentconfirmation = new PaymentStatusResponse
                                                {
                                                    CustomerId = dataToVerifyOnlinePayment.customerId,
                                                    VehicleId = dataToVerifyOnlinePayment.vehicleid,
                                                    //WyzuserId = 0,
                                                    QuoteID = dataToVerifyOnlinePayment.KYCResponseQuoteID ?? "",
                                                    ProposalID = dataToVerifyOnlinePayment.KYCResponseProposalID ?? "",
                                                    CPCode = dataToVerifyOnlinePayment.CPCode ?? "",
                                                    OEMID = dataToVerifyOnlinePayment.OEMID.ToString() ?? "",
                                                    PaymentDoneDate = paymentStatus.Payment_Date ?? "",
                                                    TransactionAmount = paymentStatus.Transaction_Amount ?? "",
                                                    TransactionReferenceNumber = paymentStatus.Transaction_Reference_Number ?? "",
                                                    Status = paymentStatus.Status ?? "",
                                                    NewPolicyNumber = paymentStatus.POLICY_NO ?? "",
                                                    PreviousPolicyNumber = paymentStatus.Previous_Policy_No ?? "",
                                                    ChassisNumber = paymentStatus.Chassis_Number ?? "",
                                                    EngineNumber = paymentStatus.Engine_Number ?? "",
                                                    UpdatedDate = paymentStatus.Updated_Date ?? "",
                                                    UpdatedTime = paymentStatus.Updated_Time ?? ""
                                                };

                                                db.PaymentStatuses.Add(paymentconfirmation);

                                                logger.Info($"[SUCCESS] Online Payment Verification Response for QuoteId:{dataToVerifyOnlinePayment.KYCResponseQuoteID} and ProposalID:{dataToVerifyOnlinePayment.ProposalID} Status Saved Successfully");
                                            }
                                            await db.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            logger.Info($"[INFO] Online Payment Verification Response for QuoteId:{dataToVerifyOnlinePayment.KYCResponseQuoteID} and ProposalID:{dataToVerifyOnlinePayment.ProposalID} is empty. No Payment Data found");
                                        }
                                    }
                                    else
                                    {
                                        JObject jsonDataObj = JObject.Parse(jsonResponseContent);

                                        if (jsonDataObj.ContainsKey("Message"))
                                        {
                                            logger.Error($"[ERROR] Online Payment Verification Job API Call Failed: Response not Success | AT: {DateTime.Now}\n" +
                                                 $"Response Status Code: {responseContent.StatusCode}\n" +
                                                 $"Response Message: {jsonDataObj["Message"]}");
                                        }

                                        logger.Error($"[ERROR] Online Payment Verification Job API Call Failed: Response not Success | AT: {DateTime.Now}\n" +
                                                 $"Response Status Code: {responseContent.StatusCode}\n" +
                                                 $"Response Message: {jsonResponseContent}");

                                    }
                                }
                                logger.Info($"============= [END]   Online Payment Verification API Responses =============");
                            }
                        }
                        else
                        {
                            logger.Info($"[INFO]  No Data Found between the DateTime From {previousDayStartDateTime} - {previousDayEndDateTime} to Verify the Online Payment Status");
                        }
                        stopScheduler("OnlinePaymentVerifyAPI_Scheduler");
                    }
                }
                catch (Exception ex)
                {
                    if (ex.InnerException != null)
                    {
                        if (ex.InnerException.InnerException != null)
                        {
                            logger.Error($"[ERROR] Online Payment Verification Job API Call Failed: Main Exception | AT: {DateTime.Now}\n" +
                                         $"Exception: {ex.InnerException.InnerException.Message}\n" +
                                         $"Method: {ex.InnerException.InnerException.TargetSite}\n" +
                                         $"StackTrace: {ex.InnerException.InnerException.StackTrace}");
                        }
                        else
                        {
                            logger.Error($"[ERROR] Online Payment Verification Job API Call Failed: Main Exception | AT: {DateTime.Now}\n" +
                                        $"Error: {ex.InnerException.Message}\n" +
                                        $"Method: {ex.InnerException.TargetSite}\n" +
                                        $"StackTrace: {ex.InnerException.StackTrace}");
                        }
                    }
                    else
                    {
                        logger.Error($"[ERROR] Online Payment Verification Job API Call Failed: Main Exception | AT: {DateTime.Now}\n" +
                                    $"Error: {ex.Message}\n" +
                                    $"Method: {ex.TargetSite}\n" +
                                    $"StackTrace: {ex.StackTrace}");
                    }

                    stopScheduler("OnlinePaymentVerifyAPI_Scheduler");

                }
            //}
            logger.Info($"============= Online Payment Verification API Job Stopped ============= | AT: {DateTime.Now}\n");
        }
    }
}